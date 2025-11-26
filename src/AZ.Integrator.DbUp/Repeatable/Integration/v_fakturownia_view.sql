DROP VIEW IF EXISTS integration.fakturownia_view;

CREATE OR REPLACE VIEW integration.fakturownia_view
AS
SELECT
    tenant_id,
    source_system_id,
    api_key,
    api_url,
    display_name,
    is_enabled,
    created_at,
    updated_at,

    is_deleted,
    deleted_at,
    deleted_by
FROM integration.fakturownia;