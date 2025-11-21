DROP VIEW IF EXISTS integration.erli_view;

CREATE OR REPLACE VIEW integration.erli_view
AS
SELECT
    tenant_id,
    source_system_id,
    api_key,
    display_name,
    is_enabled,
    created_at,
    updated_at
FROM integration.erli;