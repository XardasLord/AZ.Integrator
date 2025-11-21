START TRANSACTION;

DROP VIEW IF EXISTS integration.allegro_accounts_view;
DROP VIEW IF EXISTS integration.erli_view;

CREATE TABLE integration.fakturownia (
    tenant_id uuid NOT NULL,
    source_system_id varchar(100) NOT NULL,
    api_key text NOT NULL,
    api_url text NOT NULL,
    display_name text NOT NULL,
    is_enabled boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NOT NULL DEFAULT now(),
    PRIMARY KEY (tenant_id, source_system_id)
);

CREATE TABLE integration.inpost (
    tenant_id uuid NOT NULL,
    access_token text NOT NULL,
    organization_id numeric NOT NULL,
    display_name text NOT NULL,
    is_enabled boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz NOT NULL DEFAULT now(),
    PRIMARY KEY (tenant_id, organization_id)
);

ALTER TABLE integration.allegro
    ADD COLUMN is_enabled boolean NOT NULL DEFAULT true,
    ADD COLUMN display_name text NOT NULL DEFAULT '',
    ADD COLUMN created_at timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN updated_at timestamptz NOT NULL DEFAULT now(),
    ALTER COLUMN tenant_id type uuid using tenant_id::uuid;

ALTER TABLE integration.erli
    ADD COLUMN is_enabled boolean NOT NULL DEFAULT true,
    ADD COLUMN display_name text NOT NULL DEFAULT '',
    ADD COLUMN created_at timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN updated_at timestamptz NOT NULL DEFAULT now(),
    ALTER COLUMN tenant_id type uuid using tenant_id::uuid;

ALTER TABLE integration.shopify
    ADD COLUMN is_enabled boolean NOT NULL DEFAULT true,
    ADD COLUMN display_name text NOT NULL DEFAULT '',
    ADD COLUMN created_at timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN updated_at timestamptz NOT NULL DEFAULT now(),
    ALTER COLUMN tenant_id type uuid using tenant_id::uuid;


COMMIT;