START TRANSACTION;

DROP VIEW IF EXISTS public.invoices_view;

ALTER TABLE public.invoices SET SCHEMA billing;

COMMIT;