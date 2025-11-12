START TRANSACTION;

CREATE TABLE IF NOT EXISTS platform.feature_flags (
    code        text PRIMARY KEY,
    description text NOT NULL
);

CREATE TABLE IF NOT EXISTS platform.tenant_feature_flags (
    tenant_id   uuid NOT NULL,
    code        text NOT NULL REFERENCES platform.feature_flags(code) ON DELETE CASCADE,
    enabled     boolean NOT NULL DEFAULT false,
    modified_at timestamptz NOT NULL DEFAULT now(),
    modified_by uuid NOT NULL,
    PRIMARY KEY (tenant_id, code)
);

COMMIT;