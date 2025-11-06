DROP VIEW IF EXISTS procurement.suppliers_view;

CREATE OR REPLACE VIEW procurement.suppliers_view
AS
SELECT
    id,
    tenant_id,
    name,
    telephone_number,
    created_at,
    created_by,
    modified_at,
    modified_by
FROM procurement.suppliers;