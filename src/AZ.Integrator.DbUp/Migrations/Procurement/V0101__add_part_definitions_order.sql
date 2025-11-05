START TRANSACTION;

CREATE TABLE IF NOT EXISTS procurement.orders (
    id bigserial NOT NULL PRIMARY KEY,
    tenant_id uuid NOT NULL,
    number varchar(100) NOT NULL,
    supplier_id bigint NOT NULL,
    status integer NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by uuid NOT NULL,
    modified_at timestamp with time zone NOT NULL,
    modified_by uuid NOT NULL,

    CONSTRAINT fk_orders FOREIGN KEY (supplier_id) REFERENCES procurement.suppliers(id)
);

CREATE TABLE IF NOT EXISTS procurement.order_furniture_lines (
    id bigserial NOT NULL PRIMARY KEY,
    order_id bigint NOT NULL,
    tenant_id uuid NOT NULL,
    furniture_code varchar(100) NOT NULL,
    furniture_version integer NOT NULL,
    quantity_ordered integer NOT NULL,

    CONSTRAINT fk_order_furniture_lines__order_id FOREIGN KEY (order_id) REFERENCES procurement.orders(id),
    CONSTRAINT fk_order_furniture_lines__furniture_code FOREIGN KEY (furniture_code, tenant_id) REFERENCES catalog.furniture_models(furniture_code, tenant_id)
);

CREATE TABLE IF NOT EXISTS procurement.order_furniture_part_lines (
    id bigserial NOT NULL PRIMARY KEY,
    order_furniture_line_id bigint NOT NULL,
    part_name varchar(100) NOT NULL,
    quantity integer NOT NULL,
    additional_info varchar(100) NULL,
    length_mm integer NOT NULL,
    width_mm integer NOT NULL,
    thickness_mm integer NOT NULL,
    edge_band_length_sides smallint NOT NULL DEFAULT 0,
    edge_band_width_sides smallint NOT NULL DEFAULT 0,

    CONSTRAINT fk_order_furniture_part_lines__order_furniture_line_id FOREIGN KEY (order_furniture_line_id) REFERENCES procurement.order_furniture_lines(id)
);

COMMIT;