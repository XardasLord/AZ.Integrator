START TRANSACTION;

-- Drop views that depend on the tables being altered
DROP VIEW IF EXISTS public.allegro_accounts_view;


-- Rename table allegro_accounts to allegro
ALTER TABLE account.allegro_accounts RENAME TO allegro;


-- Add new column source_system_id
ALTER TABLE account.allegro
    ADD COLUMN source_system_id VARCHAR(100) NULL;

ALTER TABLE account.erli
    ADD COLUMN source_system_id VARCHAR(100) NULL;

ALTER TABLE account.shopify
    ADD COLUMN source_system_id VARCHAR(100) NULL;


-- Copy tenant_id to source_system_id
UPDATE account.allegro SET source_system_id = tenant_id;
UPDATE account.erli SET source_system_id = tenant_id;
UPDATE account.shopify SET source_system_id = tenant_id;


-- Remove old primary keys
ALTER TABLE account.allegro DROP CONSTRAINT "PK_allegro_accounts";
ALTER TABLE account.erli DROP CONSTRAINT "PK_erli";
ALTER TABLE account.shopify DROP CONSTRAINT "PK_shopify";


-- Add new primary keys
ALTER TABLE account.allegro
    ADD CONSTRAINT "PK_allegro"
    PRIMARY KEY (tenant_id, source_system_id);

ALTER TABLE account.erli
    ADD CONSTRAINT "PK_erli"
        PRIMARY KEY (tenant_id, source_system_id);

ALTER TABLE account.shopify
    ADD CONSTRAINT "PK_shopify"
        PRIMARY KEY (tenant_id, source_system_id);

COMMIT;