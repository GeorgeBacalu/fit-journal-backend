# Azure delivery through GitHub Actions

Every pipeline restores from committed NuGet lock files, rejects known package vulnerabilities, builds Release, runs the full unit/integration suite, publishes JUnit and Cobertura reports, and packages both the API ZIP and an EF Core migration bundle.

On `dev`, the deployment job exchanges a GitHub OIDC token for the user-assigned Azure delivery identity. No Azure client secret is stored in GitHub. The job deploys the tested ZIP and polls `/health/ready`; the API applies pending EF migrations at startup through its own managed identity.

## Protected GitHub environment

Create an `azure-dev` environment in both repositories, protect it as appropriate, and populate these environment variables from `azd env get-values` after infrastructure provisioning:

| Variable | Source |
|----------|--------|
| `AZURE_CLIENT_ID` | Delivery identity client ID |
| `AZURE_TENANT_ID` | Azure tenant ID |
| `AZURE_SUBSCRIPTION_ID` | Azure subscription ID |
| `AZURE_RESOURCE_GROUP` | AZD resource group output |
| `SERVICE_API_NAME` | API App Service name output |
| `API_URL` | API URL output |

The infrastructure creates audience `api://AzureADTokenExchange` credentials scoped to each repository's `azure-dev` environment:

```text
repo:GeorgeBacalu/fit-journal-backend:environment:azure-dev
repo:GeorgeBacalu/fit-journal-frontend:environment:azure-dev
```

Protect the `dev` branch and add deployment reviewers to the environment if approval is required. The delivery identity has Website Contributor only on the API and frontend App Services; it cannot read Key Vault secrets, Storage data, Azure SQL data, or Azure OpenAI content.

## Deployment behavior

- `quality` uses locked restore, NuGet audit, a zero-warning Release build, and the full test suite. It retains JUnit XML and Cobertura coverage artifacts even when tests fail.
- `package` publishes the API and creates a migration bundle as a recovery artifact only after quality succeeds.
- `deploy` downloads that immutable artifact, signs in through GitHub OIDC, deploys synchronously, and requires readiness within five minutes.
- EF migrations run only inside the single API instance after deployment. The SQL post-provision hook grants the managed identity the required data and DDL roles.

OAuth, JWT, and SMTP values are provisioned into Key Vault by Bicep and are never CI variables or job artifacts.
