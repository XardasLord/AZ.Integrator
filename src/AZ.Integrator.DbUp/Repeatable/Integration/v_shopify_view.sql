DROP VIEW IF EXISTS integration.shopify_view;

CREATE OR REPLACE VIEW integration.shopify_view
AS
SELECT
    tenant_id,
    source_system_id,
    api_url,
    admin_api_token,
    display_name,
    is_enabled,
    created_at,
    updated_at
FROM integration.shopify;