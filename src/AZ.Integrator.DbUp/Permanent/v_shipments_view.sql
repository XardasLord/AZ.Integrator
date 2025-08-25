DROP VIEW IF EXISTS shipments_view;
DROP VIEW IF EXISTS inpost_shipments_view;
DROP VIEW IF EXISTS dpd_shipments_view;

CREATE OR REPLACE VIEW dpd_shipments_view
AS
SELECT
    session_id,
    external_order_number,
    created_at,
    tenant_id
FROM dpd_shipments;

CREATE OR REPLACE VIEW inpost_shipments_view
AS
SELECT
    number,
    external_order_number,
    created_at,
    tenant_id
FROM inpost_shipments;

CREATE OR REPLACE VIEW shipments_view
AS
SELECT
    number as shipment_number,
    external_order_number,
    'INPOST' as shipment_provider,
    created_at,
    tenant_id
FROM inpost_shipments

UNION ALL

SELECT
    session_id::text as shipment_number,
    external_order_number,
    'DPD' as shipment_provider,
    created_at,
    tenant_id
FROM dpd_shipments;