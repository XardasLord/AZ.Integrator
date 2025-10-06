START TRANSACTION;

ALTER TABLE billing.invoices
    ADD COLUMN source_system_id text;

UPDATE billing.invoices SET source_system_id = tenant_id;

ALTER TABLE billing.invoices
    ALTER COLUMN source_system_id SET NOT NULL;

COMMIT;