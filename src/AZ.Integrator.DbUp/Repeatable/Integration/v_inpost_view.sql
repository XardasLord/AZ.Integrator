DROP VIEW IF EXISTS integration.inpost_view;

CREATE OR REPLACE VIEW integration.inpost_view
AS
SELECT
    tenant_id,
    organization_id,
    access_token,
    display_name,
    is_enabled,
    created_at,
    updated_at
FROM integration.inpost;