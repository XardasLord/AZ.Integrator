DROP VIEW IF EXISTS catalog.part_definition_view;

CREATE OR REPLACE VIEW catalog.part_definition_view
AS
SELECT
    id,
    furniture_code,
    tenant_id,
    name,
    quantity,
    additional_info,
    length_mm,
    width_mm,
    thickness_mm,
    edge_band_length_sides,
    edge_band_width_sides
FROM catalog.part_definitions;