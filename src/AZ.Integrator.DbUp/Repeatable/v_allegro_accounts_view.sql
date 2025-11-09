DROP VIEW IF EXISTS account.allegro_accounts_view;

CREATE OR REPLACE VIEW account.allegro_accounts_view
AS
SELECT
    tenant_id,
    source_system_id,
    access_token,
    refresh_token,
    expires_at,
    client_id,
    client_secret,
    redirect_uri
FROM account.allegro;