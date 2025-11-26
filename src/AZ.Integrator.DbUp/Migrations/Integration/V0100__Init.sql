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
    is_deleted boolean NOT NULL DEFAULT false,
    deleted_by uuid DEFAULT NULL,
    deleted_at timestamptz DEFAULT NULL,
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
    sender_name text NOT NULL,
    sender_company_name text NOT NULL,
    sender_first_name text NOT NULL,
    sender_last_name text NOT NULL,
    sender_email text NOT NULL,
    sender_phone text NOT NULL,
    sender_address_street text NOT NULL,
    sender_address_building_number text NOT NULL,
    sender_address_city text NOT NULL,
    sender_address_post_code text NOT NULL,
    sender_address_country_code text NOT NULL,
    is_deleted boolean NOT NULL DEFAULT false,
    deleted_by uuid DEFAULT NULL,
    deleted_at timestamptz DEFAULT NULL,
    PRIMARY KEY (tenant_id, organization_id)
);

ALTER TABLE integration.allegro
    ADD COLUMN is_enabled boolean NOT NULL DEFAULT true,
    ADD COLUMN display_name text NOT NULL DEFAULT '',
    ADD COLUMN created_at timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN updated_at timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN is_deleted boolean NOT NULL DEFAULT false,
    ADD COLUMN deleted_by uuid DEFAULT NULL,
    ADD COLUMN deleted_at timestamptz DEFAULT NULL,
    ALTER COLUMN tenant_id type uuid using tenant_id::uuid;

ALTER TABLE integration.erli
    ADD COLUMN is_enabled boolean NOT NULL DEFAULT true,
    ADD COLUMN display_name text NOT NULL DEFAULT '',
    ADD COLUMN created_at timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN updated_at timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN is_deleted boolean NOT NULL DEFAULT false,
    ADD COLUMN deleted_by uuid DEFAULT NULL,
    ADD COLUMN deleted_at timestamptz DEFAULT NULL,
    ALTER COLUMN tenant_id type uuid using tenant_id::uuid;

ALTER TABLE integration.shopify
    ADD COLUMN is_enabled boolean NOT NULL DEFAULT true,
    ADD COLUMN display_name text NOT NULL DEFAULT '',
    ADD COLUMN created_at timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN updated_at timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN is_deleted boolean NOT NULL DEFAULT false,
    ADD COLUMN deleted_by uuid DEFAULT NULL,
    ADD COLUMN deleted_at timestamptz DEFAULT NULL,
    ALTER COLUMN tenant_id type uuid using tenant_id::uuid;

COMMIT;