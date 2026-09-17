# Azure Deployment Plan

> **Status:** Ready for Validation

## Project overview

Modernize the existing FitJournal Angular SPA and ASP.NET Core API for a small, cost-optimized Azure portfolio environment. The target is the `Visual Studio Professional Subscription` (`3582a367-d2ed-439c-96b6-2fa7ee0f7cf7`) in Sweden Central.

The application must provide JWT/refresh authentication, password reset/change, Google and Microsoft OAuth, role-based administration, fitness journal CRUD, Chart.js analytics, Azure Blob-backed exercise media, contextual athlete advice through Azure OpenAI, automated tests, and GitHub Actions CI/CD.

## Components

| Component | Technology | Azure target |
|-----------|------------|--------------|
| API and domain services | ASP.NET Core / .NET 10, EF Core 10 | Linux App Service |
| SPA | Angular 21, PrimeNG, Chart.js 4 | Linux App Service |
| Persistence | SQL Server provider | Azure SQL Database Basic |
| Exercise media | Azure Storage SDK | Blob Storage Standard LRS |
| Athlete advice | Azure OpenAI REST integration | `gpt-4.1-mini` GlobalStandard |
| Monitoring | Serilog and platform telemetry | Application Insights + Log Analytics |

## Selected recipe

Azure Developer CLI with subscription-scope Bicep. GitHub Actions is the CI/CD orchestrator and authenticates through OIDC federation rather than a client secret.

## Architecture

- One Linux Basic B1 App Service plan hosts the API and SPA.
- Azure SQL uses unconditional Entra-only authentication and a Basic 5-DTU database.
- The API uses a system-assigned managed identity for SQL, Blob Storage, Key Vault references, and Azure OpenAI.
- Key Vault stores JWT, OAuth, and SMTP secrets with RBAC, soft delete, and purge protection.
- Blob shared-key access is disabled. The `exercise-media` container allows public reads for randomly named public exercise assets; writes require managed identity.
- Azure OpenAI disables local authentication and deploys `gpt-4.1-mini` version `2025-04-14`, GlobalStandard capacity 10, with the default Microsoft content policy.
- A user-assigned identity with GitHub environment federated credentials receives Website Contributor only on the two App Services.
- App Service health probes use `/health/ready` for the API and `/` for the SPA.

## Provisioning limits

Quota checks completed before generation: Storage accounts 0/250, Azure OpenAI accounts 0/30, and `gpt-4.1-mini` GlobalStandard 0/5000 units. The deployment requests one B1 plan, two sites, one Basic SQL database, one storage account, one Key Vault, one Log Analytics workspace, one Application Insights resource, one Azure OpenAI account/deployment, and one delivery identity.

## Security decisions

- No SQL authentication fallback or password properties are permitted.
- SQL connection strings use `Authentication=Active Directory Default`, encryption, and certificate validation.
- Production Azure SDK calls use `ManagedIdentityCredential`; local development uses `DefaultAzureCredential`.
- App Service resolves sensitive application values through Key Vault references.
- Azure OpenAI local/API-key authentication and Storage shared-key authentication are disabled.
- HTTPS-only, TLS 1.2+, FTPS disabled, production CORS limited to the SPA origin, and Swagger limited to Development.
- OAuth exchange codes are persisted as hashes in SQL, expire after one minute, and are atomically consumed once.
- GitHub Actions deploys use OIDC federation with site-scoped permissions.

## Research summary

- Azure App Service: both sites require the `azd-service-name` tag; App Insights auto-instrumentation uses the connection string and extension settings.
- Azure SQL: Entra-only administrator configuration and the provided post-provision scripts are mandatory for managed-identity data-plane grants.
- Azure Storage: use resource-scoped `Storage Blob Data Contributor`; no account keys are exposed.
- Key Vault: use RBAC, purge protection, and managed-identity Key Vault references.
- Azure OpenAI: live quota/model checks confirmed Sweden Central availability for the selected version/SKU/capacity.

## Generated artifacts

| File | Purpose | Status |
|------|---------|--------|
| `azure.yaml` | API/SPA services and SQL post-provision hooks | Generated |
| `infra/main.bicep` | Subscription/resource-group orchestration and outputs | Generated |
| `infra/main.parameters.json` | AZD environment parameter mapping | Generated |
| `infra/modules/*.bicep` | Monitoring, data, AI, hosting, identities, and RBAC | Generated |
| `scripts/grant-sql-access.ps1` / `.sh` | Idempotent SQL data-plane grants | Generated from required templates |
| `infra/README.md` | Secure environment and deployment inputs | Generated |

## Execution status

- [x] Backend automated test gate repaired and merged to `dev`
- [x] Managed-identity runtime, SQL OAuth exchange store, health checks, and media validation merged to `dev`
- [x] Vulnerable backend dependencies upgraded and merged to `dev`
- [x] Azure infrastructure branch validated and merged to `dev`
- [x] Backend GitHub Actions OIDC delivery branch implemented and merged to `dev`
- [x] Frontend GitHub Actions OIDC delivery/configuration branch implemented and merged to `dev`
- [x] Plan updated to `Ready for Validation`
- [ ] `azure-validate` completed

### All validation checks pass

- [x] 1. AZD Installation
- [x] 2. Schema Validation
- [ ] 3. Environment Setup
- [x] 4. Authentication Check
- [x] 5. Subscription/Location Check
- [ ] 6. Aspire Pre-Provisioning Checks (not applicable)
- [ ] 7. Provision Preview
- [x] 8. Build Verification
- [ ] 9. Docker Build Context Validation (not applicable)
- [ ] 10. Package Validation
- [x] 11. Azure Policy Validation
- [ ] 12. Aspire Post-Provisioning Checks (not applicable)

## Section 7: Validation Proof

Validation is in progress. AZD environment creation, AZD provision preview, and final package validation remain open and require the confirmed environment name. The status will not change to `Validated` until those checks pass.

## Current evidence

- Azure Developer CLI 1.30.0 is installed and authenticated as `george.bacalu@endava.com`.
- Subscription check: `Visual Studio Professional Subscription` (`3582a367-d2ed-439c-96b6-2fa7ee0f7cf7`) in tenant `0b3fc178-b730-4e8b-9843-e81259237b77`; target location is `swedencentral`.
- Bicep compilation: pass with no template warnings.
- ARM subscription-scope validation and what-if: `Succeeded` in `swedencentral` using non-secret validation placeholders; the preview is create-only with no resource modifications or deletions.
- AZD project discovery: both `api` and sibling `web` projects resolve successfully.
- Azure Policy review: no deny assignment blocks the planned resource types; applicable security policies are audit/deploy-if-not-exists.
- Final backend verification: zero-warning Release build, 59 integration tests, and 94 unit tests passed.
- Final frontend verification: 25 test files with 116 tests passed and the Angular production build succeeded.
- API publish package: succeeded with 88 files. EF migration bundle verification is still open because the Windows host locked the tool's temporary directory during cleanup; the CI command runs on Ubuntu.

No billable resources are created during preparation or validation. Provisioning and deployment require a separate explicit request after real OAuth and SMTP inputs are configured.
