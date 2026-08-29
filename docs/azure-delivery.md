# Azure delivery

The GitLab pipeline builds and tests the .NET solution for every commit. On `dev`, the database migration and App Service deployment jobs are manual so schema changes and releases remain deliberate.

Configure these masked GitLab variables in the project:

- `AZURE_CLIENT_ID`, `AZURE_CLIENT_SECRET`, `AZURE_TENANT_ID`
- `AZURE_RESOURCE_GROUP`, `AZURE_APP_SERVICE_NAME`
- `AZURE_SQL_CONNECTION_STRING`
- `AZURE_SQL_SECRET_URI`, `AZURE_AUTH_SECRET_URI`
- `AZURE_AUTH_ISSUER`, `AZURE_AUTH_AUDIENCE`

The deployed App Service must have a system-assigned managed identity with `Key Vault Secrets User` access on the Key Vault. App Service resolves the `@Microsoft.KeyVault(...)` references at runtime, so database credentials and JWT signing keys never enter GitLab artifacts or source control. `AZURE_SQL_CONNECTION_STRING` is used only by the manual migration job and should be stored as a protected masked variable.
