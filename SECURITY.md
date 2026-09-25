# Security Policy

## Scope

AshkanAQMS is an engineering/portfolio project and should be security-reviewed before deployment on an operational monitoring network.

## Secrets and credentials

Do not commit analyzer passwords, database credentials, API keys, certificates, station VPN details, private IP inventories or production configuration exports. The application includes Windows DPAPI support for protected analyzer credentials, but deployment-specific secret handling must still be reviewed.

## Reporting a vulnerability

Please report security issues privately to the repository owner rather than opening a public issue containing exploit details or credentials.

## Deployment guidance

Use least-privilege Windows/database accounts, segment analyzer networks, restrict inbound management access, back up configuration, retain audit logs, and validate every vendor-specific remote/control command before enabling it in a live station.

The RBAC/Access Governance surface in this repository is a design blueprint unless backed by a production identity provider and authorization enforcement in the deployment environment.
