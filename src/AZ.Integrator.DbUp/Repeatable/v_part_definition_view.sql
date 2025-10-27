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
    thickness_mm
FROM catalog.part_definitions;