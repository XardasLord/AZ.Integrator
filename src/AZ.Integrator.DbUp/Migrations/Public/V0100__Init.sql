START TRANSACTION;

--
-- PostgreSQL database dump
--

-- Dumped from database version 15.7 (Debian 15.7-1.pgdg120+1)
-- Dumped by pg_dump version 15.7 (Debian 15.7-1.pgdg120+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: account; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA account;


--
-- Name: public; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA public;


--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA public IS 'standard public schema';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: erli; Type: TABLE; Schema: account; Owner: -
--

CREATE TABLE account.erli (
                              tenant_id text NOT NULL,
                              api_key text NOT NULL
);


--
-- Name: shopify; Type: TABLE; Schema: account; Owner: -
--

CREATE TABLE account.shopify (
                                 tenant_id text NOT NULL,
                                 api_url text NOT NULL,
                                 admin_api_token text NOT NULL
);


--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."__EFMigrationsHistory" (
                                                "MigrationId" character varying(150) NOT NULL,
                                                "ProductVersion" character varying(32) NOT NULL
);


--
-- Name: allegro_accounts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.allegro_accounts (
                                         tenant_id text NOT NULL,
                                         access_token text NOT NULL,
                                         refresh_token text NOT NULL,
                                         client_id text DEFAULT ''::text NOT NULL,
                                         client_secret text DEFAULT ''::text NOT NULL,
                                         expires_at timestamp without time zone DEFAULT '-infinity'::timestamp without time zone NOT NULL,
                                         redirect_uri text DEFAULT ''::text NOT NULL
);


--
-- Name: allegro_accounts_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.allegro_accounts_view AS
SELECT allegro_accounts.tenant_id,
       allegro_accounts.access_token,
       allegro_accounts.refresh_token,
       allegro_accounts.expires_at,
       allegro_accounts.client_id,
       allegro_accounts.client_secret,
       allegro_accounts.redirect_uri
FROM public.allegro_accounts;


--
-- Name: dpd_packages; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.dpd_packages (
                                     number bigint NOT NULL,
                                     shipment_id bigint NOT NULL
);


--
-- Name: dpd_parcels; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.dpd_parcels (
                                    number bigint NOT NULL,
                                    waybill text,
                                    package_id bigint NOT NULL
);


--
-- Name: dpd_shipments; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.dpd_shipments (
                                      session_id bigint NOT NULL,
                                      external_order_number text,
                                      created_at timestamp with time zone NOT NULL,
                                      created_by uuid NOT NULL,
                                      tenant_id text DEFAULT ''::text NOT NULL
);


--
-- Name: dpd_shipments_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.dpd_shipments_view AS
SELECT dpd_shipments.session_id,
       dpd_shipments.external_order_number,
       dpd_shipments.created_at,
       dpd_shipments.tenant_id
FROM public.dpd_shipments;


--
-- Name: inpost_parcels; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.inpost_parcels (
                                       tracking_number text NOT NULL,
                                       shipment_number text NOT NULL
);


--
-- Name: inpost_shipments; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.inpost_shipments (
                                         number text NOT NULL,
                                         external_order_number text,
                                         created_at timestamp with time zone NOT NULL,
                                         created_by uuid NOT NULL,
                                         tenant_id text DEFAULT ''::text NOT NULL
);


--
-- Name: inpost_shipments_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.inpost_shipments_view AS
SELECT inpost_shipments.number,
       inpost_shipments.external_order_number,
       inpost_shipments.created_at,
       inpost_shipments.tenant_id
FROM public.inpost_shipments;


--
-- Name: invoices; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.invoices (
                                 number text,
                                 external_order_number text NOT NULL,
                                 created_at timestamp with time zone NOT NULL,
                                 created_by uuid NOT NULL,
                                 external_id integer DEFAULT 0 NOT NULL,
                                 tenant_id text DEFAULT ''::text NOT NULL
);


--
-- Name: invoices_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.invoices_view AS
SELECT invoices.external_id,
       invoices.number,
       invoices.external_order_number,
       invoices.created_at,
       invoices.tenant_id
FROM public.invoices;


--
-- Name: shipments_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.shipments_view AS
SELECT inpost_shipments.number AS shipment_number,
       inpost_shipments.external_order_number,
       'INPOST'::text AS shipment_provider,
       inpost_shipments.created_at,
       inpost_shipments.tenant_id
FROM public.inpost_shipments
UNION ALL
SELECT (dpd_shipments.session_id)::text AS shipment_number,
       dpd_shipments.external_order_number,
       'DPD'::text AS shipment_provider,
       dpd_shipments.created_at,
       dpd_shipments.tenant_id
FROM public.dpd_shipments;


--
-- Name: stock_groups; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.stock_groups (
                                     id bigint NOT NULL,
                                     name text NOT NULL,
                                     description text NOT NULL,
                                     created_at timestamp with time zone NOT NULL,
                                     created_by text NOT NULL,
                                     created_by_id uuid NOT NULL,
                                     modified_at timestamp with time zone NOT NULL,
                                     modified_by uuid NOT NULL
);


--
-- Name: stock_groups_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.stock_groups ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.stock_groups_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
    );


--
-- Name: stock_groups_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.stock_groups_view AS
SELECT stock_groups.id,
       stock_groups.name,
       stock_groups.description,
       stock_groups.created_at,
       stock_groups.created_by,
       stock_groups.modified_at,
       stock_groups.modified_by
FROM public.stock_groups;


--
-- Name: stock_logs; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.stock_logs (
                                   id bigint NOT NULL,
                                   change_quantity integer NOT NULL,
                                   created_at timestamp with time zone NOT NULL,
                                   created_by text NOT NULL,
                                   package_code text NOT NULL,
                                   created_by_id uuid DEFAULT '00000000-0000-0000-0000-000000000000'::uuid NOT NULL,
                                   status integer DEFAULT 0 NOT NULL
);


--
-- Name: stock_logs_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.stock_logs ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.stock_logs_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
    );


--
-- Name: stock_logs_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.stock_logs_view AS
SELECT stock_logs.id,
       stock_logs.change_quantity,
       stock_logs.created_at,
       stock_logs.created_by,
       stock_logs.package_code,
       stock_logs.status
FROM public.stock_logs;


--
-- Name: stocks; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.stocks (
                               package_code text NOT NULL,
                               quantity integer NOT NULL,
                               group_id bigint,
                               threshold integer DEFAULT 0 NOT NULL
);


--
-- Name: stocks_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.stocks_view AS
SELECT stocks.package_code,
       stocks.quantity,
       stocks.group_id,
       stocks.threshold
FROM public.stocks;


--
-- Name: tag_parcel_templates; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tag_parcel_templates (
                                             tag text NOT NULL,
                                             created_at timestamp with time zone NOT NULL,
                                             created_by uuid NOT NULL
);


--
-- Name: tag_parcel_templates_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.tag_parcel_templates_view AS
SELECT tag_parcel_templates.tag,
       tag_parcel_templates.created_at,
       tag_parcel_templates.created_by
FROM public.tag_parcel_templates;


--
-- Name: tag_parcels; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tag_parcels (
                                    id bigint NOT NULL,
                                    length double precision,
                                    width double precision,
                                    height double precision,
                                    weight double precision NOT NULL,
                                    tag text NOT NULL
);


--
-- Name: tag_parcels_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.tag_parcels ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tag_parcels_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
    );


--
-- Name: tag_parcels_view; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.tag_parcels_view AS
SELECT tag_parcels.id,
       tag_parcels.tag,
       tag_parcels.length,
       tag_parcels.width,
       tag_parcels.height,
       tag_parcels.weight
FROM public.tag_parcels;


--
-- Name: erli PK_erli; Type: CONSTRAINT; Schema: account; Owner: -
--

ALTER TABLE ONLY account.erli
    ADD CONSTRAINT "PK_erli" PRIMARY KEY (tenant_id);


--
-- Name: shopify PK_shopify; Type: CONSTRAINT; Schema: account; Owner: -
--

ALTER TABLE ONLY account.shopify
    ADD CONSTRAINT "PK_shopify" PRIMARY KEY (tenant_id);


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: allegro_accounts PK_allegro_accounts; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.allegro_accounts
    ADD CONSTRAINT "PK_allegro_accounts" PRIMARY KEY (tenant_id);


--
-- Name: dpd_packages PK_dpd_packages; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.dpd_packages
    ADD CONSTRAINT "PK_dpd_packages" PRIMARY KEY (number);


--
-- Name: dpd_parcels PK_dpd_parcels; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.dpd_parcels
    ADD CONSTRAINT "PK_dpd_parcels" PRIMARY KEY (number);


--
-- Name: dpd_shipments PK_dpd_shipments; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.dpd_shipments
    ADD CONSTRAINT "PK_dpd_shipments" PRIMARY KEY (session_id);


--
-- Name: inpost_parcels PK_inpost_parcels; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.inpost_parcels
    ADD CONSTRAINT "PK_inpost_parcels" PRIMARY KEY (tracking_number);


--
-- Name: inpost_shipments PK_inpost_shipments; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.inpost_shipments
    ADD CONSTRAINT "PK_inpost_shipments" PRIMARY KEY (number);


--
-- Name: invoices PK_invoices; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.invoices
    ADD CONSTRAINT "PK_invoices" PRIMARY KEY (external_id);


--
-- Name: stock_groups PK_stock_groups; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.stock_groups
    ADD CONSTRAINT "PK_stock_groups" PRIMARY KEY (id);


--
-- Name: stock_logs PK_stock_logs; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.stock_logs
    ADD CONSTRAINT "PK_stock_logs" PRIMARY KEY (id);


--
-- Name: stocks PK_stocks; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.stocks
    ADD CONSTRAINT "PK_stocks" PRIMARY KEY (package_code);


--
-- Name: tag_parcel_templates PK_tag_parcel_templates; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tag_parcel_templates
    ADD CONSTRAINT "PK_tag_parcel_templates" PRIMARY KEY (tag);


--
-- Name: tag_parcels PK_tag_parcels; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tag_parcels
    ADD CONSTRAINT "PK_tag_parcels" PRIMARY KEY (id);


--
-- Name: IX_dpd_packages_shipment_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_dpd_packages_shipment_id" ON public.dpd_packages USING btree (shipment_id);


--
-- Name: IX_dpd_parcels_package_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_dpd_parcels_package_id" ON public.dpd_parcels USING btree (package_id);


--
-- Name: IX_inpost_parcels_shipment_number; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_inpost_parcels_shipment_number" ON public.inpost_parcels USING btree (shipment_number);


--
-- Name: IX_stock_logs_package_code; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_stock_logs_package_code" ON public.stock_logs USING btree (package_code);


--
-- Name: IX_tag_parcels_tag; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_tag_parcels_tag" ON public.tag_parcels USING btree (tag);


--
-- Name: dpd_packages FK_dpd_packages_dpd_shipments_shipment_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.dpd_packages
    ADD CONSTRAINT "FK_dpd_packages_dpd_shipments_shipment_id" FOREIGN KEY (shipment_id) REFERENCES public.dpd_shipments(session_id) ON DELETE CASCADE;


--
-- Name: dpd_parcels FK_dpd_parcels_dpd_packages_package_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.dpd_parcels
    ADD CONSTRAINT "FK_dpd_parcels_dpd_packages_package_id" FOREIGN KEY (package_id) REFERENCES public.dpd_packages(number) ON DELETE CASCADE;


--
-- Name: inpost_parcels FK_inpost_parcels_inpost_shipments_shipment_number; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.inpost_parcels
    ADD CONSTRAINT "FK_inpost_parcels_inpost_shipments_shipment_number" FOREIGN KEY (shipment_number) REFERENCES public.inpost_shipments(number) ON DELETE CASCADE;


--
-- Name: stock_logs FK_stock_logs_stocks_package_code; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.stock_logs
    ADD CONSTRAINT "FK_stock_logs_stocks_package_code" FOREIGN KEY (package_code) REFERENCES public.stocks(package_code) ON DELETE CASCADE;


--
-- Name: tag_parcels FK_tag_parcels_tag_parcel_templates_tag; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tag_parcels
    ADD CONSTRAINT "FK_tag_parcels_tag_parcel_templates_tag" FOREIGN KEY (tag) REFERENCES public.tag_parcel_templates(tag) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--



COMMIT;