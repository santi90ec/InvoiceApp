# Reglas de Seguridad para Pepe

## Principio Fundamental

**Privilegio Mínimo:** Solo accedes a lo estrictamente necesario para tu trabajo.

**Zero Trust:** Cada operación debe validarse. No asumas permisos.

---

## Prohibiciones Absolutas

### 🚨 NUNCA Hagas Esto:

#### Secrets y Credenciales

❌ Leer appsettings.json en modo write  
❌ Commitear API keys, tokens, passwords  
❌ Hardcodear credenciales en código  
❌ Logear secrets (OpenAI keys, tokens SRI)  
❌ Exponer secrets en mensajes de error

**Ejemplo de violación:**
```csharp
// ❌ MAL
var apiKey = "sk-proj-abc123xyz...";
_logger.LogInformation("Using key: {Key}", apiKey);
```

**Correcto:**
```csharp
// ✅ BIEN
var apiKey = _configuration["OpenAI:ApiKey"];
_logger.LogInformation("OpenAI client initialized");
```

---

#### Datos de Usuario (PII)

❌ Leer/escribir en /uploads (facturas XML usuarios)  
❌ Leer/escribir en /evidence (screenshots SRI)  
❌ Logear nombres, cédulas, direcciones  
❌ Exponer datos fiscales en responses públicos

**Ejemplo de violación:**
```csharp
// ❌ MAL
_logger.LogInformation(
    "Procesando factura de {Nombre} cédula {Cedula}",
    invoice.Comprador.Nombre,
    invoice.Comprador.Cedula
);
```

**Correcto:**
```csharp
// ✅ BIEN
_logger.LogInformation(
    "Procesando factura {InvoiceId}",
    invoice.Id
);
```

---

#### Sistema de Archivos

❌ Escribir fuera de /src, /tests, /docs  
❌ Ejecutar comandos destructivos (rm -rf, format, etc)  
❌ Modificar archivos de gobernanza (/pepe/memory/decisions.md)  
❌ Acceder a directorios del sistema (/etc, /var, /home)

**Validación pre-write:**
```csharp
// ✅ BIEN - Validar path antes de escribir
public void WriteFile(string path, string content)
{
    var allowedDirs = new[] { "/src", "/tests", "/docs" };
    var normalizedPath = Path.GetFullPath(path);
    
    if (!allowedDirs.Any(d => normalizedPath.StartsWith(d)))
        throw new SecurityException($"Path not allowed: {path}");
    
    File.WriteAllText(normalizedPath, content);
}
```

---

#### Git y CI/CD

❌ Modificar .github/workflows sin aprobación  
❌ Cambiar branch protection rules  
❌ Mergear PRs (solo S4 aprueba)  
❌ Force push a main  
❌ Commitear archivos binarios grandes

---

## Validaciones Obligatorias

### Pre-Commit Checklist

Antes de CADA commit, valida:

- [ ] ¿Hay TODO: sin contexto? → Agregar ticket o descripción
- [ ] ¿Hay HACK: o FIXME:? → Reportar a S4
- [ ] ¿Hardcoded strings sensibles? → Mover a config
- [ ] ¿Logs con datos personales? → Redactar
- [ ] ¿Archivos en .gitignore commitados? → Eliminar
- [ ] ¿Build warnings presentes? → Resolver
- [ ] ¿Tests skipped o comentados? → Habilitar o reportar

### Pre-PR Checklist

Antes de CADA Pull Request:

- [ ] dotnet build --no-restore → 0 errors, 0 warnings
- [ ] dotnet test --no-build → All tests passing
- [ ] git status → No archivos sin trackear
- [ ] git log → Commits descriptivos
- [ ] PR template completo → Descripción, issue, validación

---

## Manejo de Errores Sensibles

### ❌ MAL - Exponer detalles internos:
```csharp
catch (Exception ex)
{
    // ❌ Expone paths internos, SQL, stack traces
    return StatusCode(500, new { 
        Error = ex.Message,
        StackTrace = ex.StackTrace
    });
}
```

### ✅ BIEN - Error seguro:
```csharp
catch (Exception ex)
{
    _logger.LogError(ex, "Error procesando factura {InvoiceId}", invoiceId);
    
    return StatusCode(500, new { 
        Error = "Error procesando factura",
        RequestId = Activity.Current?.Id
    });
}
```

---

## Logging Seguro

### Reglas de Oro:

**NUNCA loguees:**
- Passwords, tokens, API keys
- Números de cédula completos
- Emails completos
- Direcciones completas
- Números de tarjeta

**SIEMPRE loguea:**
- IDs de entidades (InvoiceId, JobId)
- Timestamps
- Resultados de operaciones (success/failure)
- Performance metrics

**REDACTA datos sensibles:**
```csharp
// ✅ BIEN
_logger.LogInformation(
    "Usuario {UserId} procesó {Count} facturas",
    userId.Substring(0, 4) + "***",
    invoiceCount
);
```

---

## Configuración Segura

### appsettings.json

**Estructura permitida:**
```json
{
  "OpenAI": {
    "ApiKey": "${OPENAI_API_KEY}",
    "Model": "gpt-4o-mini"
  },
  "Database": {
    "ConnectionString": "Data Source=sri.db"
  }
}
```

**PROHIBIDO:**
```json
{
  "OpenAI": {
    "ApiKey": "sk-proj-abc123..."
  },
  "SRI": {
    "Username": "user@test.ec",
    "Password": "Password123"
  }
}
```

---

## Variables de Entorno

### ✅ BIEN - Usar env vars:
```csharp
var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
    ?? throw new InvalidOperationException("OPENAI_API_KEY not set");
```

### ❌ MAL - Fallback inseguro:
```csharp
var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
    ?? "sk-proj-default-key";  // ❌ NUNCA hagas esto
```

---

## Dependencias Externas

### Paquetes Pre-Aprobados:

✅ Microsoft.EntityFrameworkCore.*  
✅ xUnit  
✅ Moq  
✅ FluentAssertions  
✅ Serilog.*  
✅ OpenAI (oficial)  
✅ Microsoft.Playwright

### Requieren Aprobación S4:

❓ Cualquier paquete de terceros no listado  
❓ Versiones diferentes a las del proyecto  
❓ Paquetes deprecated o unmaintained

---

## Inyección de Código

### SQL Injection (aunque usemos EF Core):
```csharp
// ❌ MAL
var query = $"SELECT * FROM Invoices WHERE Id = {invoiceId}";

// ✅ BIEN
var invoice = await _context.Invoices
    .FirstOrDefaultAsync(i => i.Id == invoiceId);
```

### Command Injection:
```csharp
// ❌ MAL
Process.Start("cmd.exe", $"/c type {filename}");

// ✅ BIEN - Validar y sanitizar
var allowedFiles = new[] { "data.txt", "config.json" };
if (!allowedFiles.Contains(filename))
    throw new SecurityException("File not allowed");
    
var safeFilename = Path.GetFileName(filename);
// ... continuar
```

---

## Path Traversal

### ❌ MAL:
```csharp
var filepath = $"/uploads/{userInput}";
var content = File.ReadAllText(filepath);
```

Si userInput = `../../etc/passwd` → lee archivo del sistema

### ✅ BIEN:
```csharp
var filename = Path.GetFileName(userInput);  // Remove path
var safePath = Path.Combine("/uploads", filename);
var fullPath = Path.GetFullPath(safePath);

if (!fullPath.StartsWith("/uploads"))
    throw new SecurityException("Path traversal detected");
    
var content = File.ReadAllText(fullPath);
```

---

## XML External Entity (XXE)

### ❌ MAL:
```csharp
var settings = new XmlReaderSettings 
{ 
    DtdProcessing = DtdProcessing.Parse  // ❌ Vulnerable
};
var reader = XmlReader.Create(stream, settings);
```

### ✅ BIEN:
```csharp
var settings = new XmlReaderSettings 
{ 
    DtdProcessing = DtdProcessing.Prohibit,  // ✅ Seguro
    XmlResolver = null
};
var reader = XmlReader.Create(stream, settings);
```

---

## Validación de Inputs

### SIEMPRE Valida:
```csharp
public Result<Invoice> ProcessInvoice(string xml)
{
    // 1. Null/empty check
    if (string.IsNullOrWhiteSpace(xml))
        return Result<Invoice>.Failure("XML vacío");
    
    // 2. Size check
    if (xml.Length > 1_000_000)  // 1MB max
        return Result<Invoice>.Failure("XML demasiado grande");
    
    // 3. Format check (básico)
    if (!xml.TrimStart().StartsWith("<"))
        return Result<Invoice>.Failure("No es XML válido");
    
    // 4. Parse con try-catch
    try
    {
        var doc = XDocument.Parse(xml);
        // ... validación schema
    }
    catch (XmlException ex)
    {
        _logger.LogWarning(ex, "XML malformado");
        return Result<Invoice>.Failure("XML malformado");
    }
}
```

---

## Rate Limiting (Futuro)

Aunque no implementado en Sprint 0, tener presente:
```csharp
// Sprint 3+: Rate limit para clasificación IA
[RateLimit(RequestsPerMinute = 10)]
public async Task<ClassificationResult> ClassifyAsync(Invoice invoice)
{
    // ...
}
```

---

## Incident Response

### Si Detectas Vulnerabilidad:

1. ❌ NO crear issue público
2. ✅ Notificar a S4 inmediatamente por canal privado
3. ✅ NO commitear evidencia de la vulnerabilidad
4. ✅ Documentar en privado:
   - Qué encontraste
   - Cómo reproducirlo
   - Impacto estimado

### Si Commiteas Secret Accidentalmente:

1. ✅ Notificar a S4 inmediatamente
2. ✅ Rotar el secret (API key, token)
3. ✅ Hacer commit que borra el secret
4. ✅ NO intentar reescribir historia de Git (force push)

---

## Checklist Final Seguridad

Antes de CADA PR, responde:

- [ ] ¿Todos los secrets están en variables de entorno?
- [ ] ¿Los logs NO contienen PII?
- [ ] ¿Los errores NO exponen stack traces a usuarios?
- [ ] ¿Validé TODOS los inputs de usuario?
- [ ] ¿Usé parameterized queries (EF Core)?
- [ ] ¿Saniticé paths antes de file operations?
- [ ] ¿Deshabillité DTD processing en XML?
- [ ] ¿NO agregué dependencias sin aprobar?

---

**Versión:** v1.0  
**Última actualización:** 2026-02-21 (Sprint 0)  
**Próxima revisión:** Sprint 2