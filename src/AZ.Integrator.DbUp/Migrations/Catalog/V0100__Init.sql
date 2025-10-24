START TRANSACTION;

CREATE TABLE IF NOT EXISTS catalog.furniture_models (
    furniture_code varchar(100) NOT NULL,
    tenant_id uuid NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by uuid NOT NULL,
    modified_at timestamp with time zone NOT NULL,
    modified_by uuid NOT NULL,
    version integer NOT NULL,
    is_deleted bool default false,
    deleted_at timestamp with time zone NULL,
    PRIMARY KEY (furniture_code, tenant_id)
);

CREATE TABLE IF NOT EXISTS catalog.part_definitions (
    id bigserial NOT NULL PRIMARY KEY,
    furniture_code varchar(100) NOT NULL,
    tenant_id uuid NOT NULL,
    name varchar(100) NOT NULL,
    color varchar(100) NULL,
    additional_info varchar(100) NULL,
    length_mm integer NOT NULL,
    width_mm integer NOT NULL,
    thickness_mm integer NOT NULL
);

COMMIT;