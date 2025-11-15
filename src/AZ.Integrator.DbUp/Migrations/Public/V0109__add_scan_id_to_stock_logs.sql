START TRANSACTION;

ALTER TABLE stock_logs ADD COLUMN scan_id text NOT NULL;
ALTER TABLE stock_logs ADD COLUMN tenant_id uuid NOT NULL;

ALTER TABLE stocks ADD COLUMN tenant_id uuid NOT NULL;
ALTER TABLE stock_groups ADD COLUMN tenant_id uuid NOT NULL;

COMMIT;