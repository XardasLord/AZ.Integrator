START TRANSACTION;

DROP VIEW IF EXISTS billing.invoices_view;

UPDATE billing.invoices SET tenant_id = '7ec2ff33-f39e-4c1f-bbf5-33c0036971f5';

alter table billing.invoices
    alter column tenant_id drop default;

alter table billing.invoices
    alter column tenant_id type uuid using tenant_id::uuid;

COMMIT;