START TRANSACTION;

CREATE TABLE IF NOT EXISTS procurement.suppliers (
    id bigserial NOT NULL PRIMARY KEY,
    tenant_id uuid NOT NULL,
    name varchar(100) NOT NULL,
    telephone_number varchar(100) NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by uuid NOT NULL,
    modified_at timestamp with time zone NOT NULL,
    modified_by uuid NOT NULL
);

CREATE TABLE IF NOT EXISTS procurement.supplier_mailboxes (
    id bigserial NOT NULL PRIMARY KEY,
    supplier_id integer NOT NULL,
    email varchar(100) NOT NULL,

    CONSTRAINT fk_suppliers FOREIGN KEY (supplier_id) REFERENCES procurement.suppliers(id)
);

COMMIT;