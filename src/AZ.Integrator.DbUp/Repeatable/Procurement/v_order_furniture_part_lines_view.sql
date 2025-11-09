DROP VIEW IF EXISTS procurement.orders_view;
DROP VIEW IF EXISTS procurement.v_order_furniture_lines_view;
DROP VIEW IF EXISTS procurement.order_furniture_part_lines_view;

CREATE OR REPLACE VIEW procurement.order_furniture_part_lines_view
AS
SELECT
    id,
    order_furniture_line_id,
    part_name,
    quantity,
    additional_info,
    length_mm,
    width_mm,
    thickness_mm,
    edge_band_length_sides,
    edge_band_width_sides
FROM procurement.order_furniture_part_lines;