START TRANSACTION;

DROP VIEW IF EXISTS public.allegro_accounts_view;

ALTER TABLE public.allegro_accounts SET SCHEMA account;

COMMIT;