START TRANSACTION;

CREATE TABLE IF NOT EXISTS platform.tenants (
    tenant_id   uuid NOT NULL PRIMARY KEY,
    name        text NOT NULL,
    is_active   boolean NOT NULL DEFAULT false,
    created_at  timestamptz NOT NULL DEFAULT now(),
    modified_at timestamptz DEFAULT null
);

COMMIT;