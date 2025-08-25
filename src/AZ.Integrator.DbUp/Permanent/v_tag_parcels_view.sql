DROP VIEW IF EXISTS tag_parcels_view;

CREATE OR REPLACE VIEW tag_parcels_view
AS
SELECT
    id,
    tag,
    length,
    width,
    height,
    weight
FROM tag_parcels;