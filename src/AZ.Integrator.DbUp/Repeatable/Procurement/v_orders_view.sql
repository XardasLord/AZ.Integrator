DROP VIEW IF EXISTS procurement.orders_view;

CREATE OR REPLACE VIEW procurement.orders_view
AS
SELECT
    id,
    tenant_id,
    number,
    supplier_id,
    additional_notes,
    status,
    created_at,
    created_by,
    modified_at,
    modified_by
FROM procurement.orders;