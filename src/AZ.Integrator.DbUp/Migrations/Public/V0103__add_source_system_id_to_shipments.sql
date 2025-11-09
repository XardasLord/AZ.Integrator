START TRANSACTION;

ALTER TABLE public.inpost_shipments
    ADD COLUMN source_system_id text;

UPDATE public.inpost_shipments SET source_system_id = tenant_id;

ALTER TABLE public.inpost_shipments
    ALTER COLUMN source_system_id SET NOT NULL;


ALTER TABLE public.dpd_shipments
    ADD COLUMN source_system_id text;

UPDATE public.dpd_shipments SET source_system_id = tenant_id;

ALTER TABLE public.dpd_shipments
    ALTER COLUMN source_system_id SET NOT NULL;

COMMIT;