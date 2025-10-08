START TRANSACTION;

DROP VIEW IF EXISTS shipments_view;
DROP VIEW IF EXISTS inpost_shipments_view;
DROP VIEW IF EXISTS dpd_shipments_view;

UPDATE public.inpost_shipments SET tenant_id = '7ec2ff33-f39e-4c1f-bbf5-33c0036971f5';
UPDATE public.dpd_shipments SET tenant_id = '7ec2ff33-f39e-4c1f-bbf5-33c0036971f5';

alter table inpost_shipments
    alter column tenant_id drop default;

alter table dpd_shipments
    alter column tenant_id drop default;

alter table inpost_shipments
    alter column tenant_id type uuid using tenant_id::uuid;

alter table dpd_shipments
    alter column tenant_id type uuid using tenant_id::uuid;

COMMIT;