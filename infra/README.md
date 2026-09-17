# FitJournal Azure infrastructure

This folder contains the AZD/Bicep definition for the two App Services, Azure SQL, Blob Storage, Key Vault, Application Insights, Log Analytics, Azure OpenAI, managed identities, GitHub Actions workload federation, and least-privilege role assignments.

## Required AZD environment values

Create an environment and select the approved Azure context:

```powershell
azd env new fitjournal-dev
azd env set AZURE_SUBSCRIPTION_ID 3582a367-d2ed-439c-96b6-2fa7ee0f7cf7
azd env set AZURE_LOCATION swedencentral
azd env set AZURE_PRINCIPAL_ID <entra-object-id>
azd env set AZURE_PRINCIPAL_NAME <entra-display-name>
azd env set AZURE_PRINCIPAL_TYPE User
```

Set these deployment inputs locally or through protected CI variables. Do not commit their values:

```powershell
azd env set JWT_SECRET <at-least-32-random-characters>
azd env set GOOGLE_CLIENT_ID <google-client-id>
azd env set GOOGLE_CLIENT_SECRET <google-client-secret>
azd env set MICROSOFT_CLIENT_ID <microsoft-client-id>
azd env set MICROSOFT_CLIENT_SECRET <microsoft-client-secret>
azd env set SMTP_USER <smtp-user>
azd env set SMTP_PASSWORD <smtp-app-password>
azd env set SMTP_MAILBOX_NAME <sender-display-name>
azd env set GITHUB_BACKEND_SUBJECT repo:GeorgeBacalu/fit-journal-backend:environment:azure-dev
azd env set GITHUB_FRONTEND_SUBJECT repo:GeorgeBacalu/fit-journal-frontend:environment:azure-dev
```

The subjects above bind the delivery identity to the protected `azure-dev` GitHub environment in each repository. If either repository is moved or renamed, update its subject before provisioning.

After provisioning, copy the non-secret deployment outputs into environment variables for the `azure-dev` GitHub environment in each repository as described in `docs/azure-delivery.md`.

## Provisioning and deployment

`azd provision` creates billable Azure resources. Run it only after validation and explicit approval. The post-provision hook grants the API managed identity `db_datareader`, `db_datawriter`, and `db_ddladmin`; both `azd` and modern `sqlcmd` must be available.

After provisioning, use `azd env get-values` to obtain `API_URL` and `WEB_URL`. Register these provider callbacks:

- Google: `${API_URL}/signin-google`
- Microsoft: `${API_URL}/signin-microsoft`

The exercise-media container intentionally permits anonymous blob reads because exercise thumbnails and demonstration videos are public application assets with random blob names. Shared-key access is disabled; uploads require the API managed identity and `Storage Blob Data Contributor`.

## Local validation

```powershell
az bicep build --file .\infra\main.bicep --stdout
azd show --output json
dotnet build .\FitJournal.slnx --configuration Release --no-restore -m:1
dotnet test .\FitJournal.slnx --configuration Release --no-build --no-restore -m:1
```

The generated SQL server is always Entra-only. Do not add SQL usernames or passwords to the templates or application settings.
