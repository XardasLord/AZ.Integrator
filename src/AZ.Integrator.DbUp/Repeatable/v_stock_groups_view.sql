DROP VIEW IF EXISTS stock_groups_view;

CREATE OR REPLACE VIEW stock_groups_view
AS
SELECT
    id,
    tenant_id,
    name,
    description,
    created_at,
    created_by,
    modified_at,
    modified_by
FROM stock_groups;