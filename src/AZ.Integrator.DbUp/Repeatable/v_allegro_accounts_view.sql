DROP VIEW IF EXISTS allegro_accounts_view;

CREATE VIEW allegro_accounts_view
AS
SELECT
    tenant_id,
    access_token,
    refresh_token,
    expires_at,
    client_id,
    client_secret,
    redirect_uri
FROM allegro_accounts;