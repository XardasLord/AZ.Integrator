START TRANSACTION;

CREATE SCHEMA IF NOT EXISTS integration;

ALTER TABLE account.allegro_accounts_view SET SCHEMA integration;
ALTER TABLE account.allegro SET SCHEMA integration;
ALTER TABLE account.erli SET SCHEMA integration;
ALTER TABLE account.shopify SET SCHEMA integration;

COMMIT;