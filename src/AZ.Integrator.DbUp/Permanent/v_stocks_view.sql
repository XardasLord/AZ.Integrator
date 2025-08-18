DROP VIEW IF EXISTS stocks_view;

CREATE OR REPLACE VIEW stocks_view
AS
SELECT
    package_code,
    quantity,
    group_id,
    threshold
FROM stocks;