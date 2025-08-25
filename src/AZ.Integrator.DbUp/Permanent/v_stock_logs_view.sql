DROP VIEW IF EXISTS stock_logs_view;

CREATE OR REPLACE VIEW stock_logs_view
AS
SELECT
    id,
    change_quantity,
    created_at,
    created_by,
    package_code,
    status
FROM stock_logs;