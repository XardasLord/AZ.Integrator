DROP VIEW IF EXISTS catalog.furniture_models_view;

CREATE OR REPLACE VIEW catalog.furniture_models_view
AS
SELECT
    furniture_code,
    tenant_id,
    created_at,
    created_by,
    modified_at,
    modified_by,
    is_deleted,
    deleted_at,
    version
FROM catalog.furniture_models;