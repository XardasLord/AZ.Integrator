DROP VIEW IF EXISTS tag_parcel_templates_view;

CREATE OR REPLACE VIEW tag_parcel_templates_view
AS
SELECT
    tag,
    created_at,
    created_by
FROM tag_parcel_templates;