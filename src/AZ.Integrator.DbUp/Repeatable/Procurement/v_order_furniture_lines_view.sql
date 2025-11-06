DROP VIEW IF EXISTS procurement.orders_view;
DROP VIEW IF EXISTS procurement.v_order_furniture_lines_view;

CREATE OR REPLACE VIEW procurement.order_furniture_lines_view
AS
SELECT
    id,
    tenant_id,
    order_id,
    furniture_code,
    furniture_version,
    quantity_ordered
FROM procurement.order_furniture_lines;