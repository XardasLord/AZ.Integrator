DROP VIEW IF EXISTS invoices_view;

CREATE OR REPLACE VIEW invoices_view
AS
SELECT
    external_id,
    number,
    external_order_number,
    created_at,
    tenant_id
FROM invoices;