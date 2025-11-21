DROP VIEW IF EXISTS integration.allegro_accounts_view;
DROP VIEW IF EXISTS integration.allegro_view;

CREATE OR REPLACE VIEW integration.allegro_view
AS
SELECT
    tenant_id,
    source_system_id,
    access_token,
    refresh_token,
    expires_at,
    client_id,
    client_secret,
    redirect_uri,
    display_name,
    is_enabled,
    created_at,
    updated_at
FROM integration.allegro;