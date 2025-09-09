START TRANSACTION;

ALTER TABLE billing.invoices
    ADD COLUMN provider INT,
    ADD COLUMN idempotency_key VARCHAR(255);

CREATE TABLE IF NOT EXISTS billing.invoice_providers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

INSERT INTO billing.invoice_providers (id, name)
VALUES (1, 'Fakturownia')
ON CONFLICT (id) DO NOTHING;

UPDATE billing.invoices
SET provider = 1
WHERE provider IS NULL;

ALTER TABLE billing.invoices
    ADD CONSTRAINT fk_invoices_provider
    FOREIGN KEY (provider)
    REFERENCES billing.invoice_providers(id);

COMMIT;