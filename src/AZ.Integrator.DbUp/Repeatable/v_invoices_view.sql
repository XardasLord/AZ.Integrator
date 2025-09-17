DROP VIEW IF EXISTS billing.invoices_view;

CREATE OR REPLACE VIEW billing.invoices_view
AS
SELECT
    external_id,
    number,
    external_order_number,
    created_at,
    tenant_id
FROM billing.invoices;