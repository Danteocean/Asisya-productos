--
-- PostgreSQL database dump
--

-- Dumped from database version 15.4
-- Dumped by pg_dump version 15.4

-- Started on 2026-04-16 09:08:43

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
-- TOC entry 2 (class 3079 OID 2442029)
-- Name: pg_trgm; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS pg_trgm WITH SCHEMA public;


--
-- TOC entry 3514 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION pg_trgm; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pg_trgm IS 'text similarity measurement and index searching based on trigrams';


--
-- TOC entry 260 (class 1255 OID 2442110)
-- Name: trigger_set_timestamp(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.trigger_set_timestamp() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$;


ALTER FUNCTION public.trigger_set_timestamp() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 216 (class 1259 OID 2442112)
-- Name: categories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.categories (
    category_id integer NOT NULL,
    category_name character varying(50) NOT NULL,
    description text,
    picture bytea,
    is_active boolean DEFAULT true,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now(),
    created_by integer,
    updated_by integer
);


ALTER TABLE public.categories OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 2442111)
-- Name: categories_category_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.categories_category_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.categories_category_id_seq OWNER TO postgres;

--
-- TOC entry 3515 (class 0 OID 0)
-- Dependencies: 215
-- Name: categories_category_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.categories_category_id_seq OWNED BY public.categories.category_id;


--
-- TOC entry 221 (class 1259 OID 2442145)
-- Name: customers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.customers (
    customer_id character(5) NOT NULL,
    company_name character varying(100) NOT NULL,
    contact_name character varying(100),
    contact_title character varying(50),
    address character varying(200),
    city character varying(50),
    region character varying(50),
    postal_code character varying(20),
    country character varying(50),
    phone character varying(25),
    fax character varying(25),
    is_active boolean DEFAULT true,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now()
);


ALTER TABLE public.customers OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 2442156)
-- Name: employees; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.employees (
    employee_id integer NOT NULL,
    last_name character varying(50) NOT NULL,
    first_name character varying(50) NOT NULL,
    title character varying(50),
    title_of_courtesy character varying(25),
    birth_date date,
    hire_date date,
    address character varying(200),
    city character varying(50),
    region character varying(50),
    postal_code character varying(20),
    country character varying(50),
    home_phone character varying(25),
    extension character varying(5),
    photo bytea,
    notes text,
    reports_to integer,
    is_active boolean DEFAULT true,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now(),
    username character varying(50),
    password_hash text
);


ALTER TABLE public.employees OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 2442155)
-- Name: employees_employee_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.employees_employee_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.employees_employee_id_seq OWNER TO postgres;

--
-- TOC entry 3516 (class 0 OID 0)
-- Dependencies: 222
-- Name: employees_employee_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.employees_employee_id_seq OWNED BY public.employees.employee_id;


--
-- TOC entry 228 (class 1259 OID 2442226)
-- Name: order_details; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_details (
    order_id integer NOT NULL,
    product_id integer NOT NULL,
    unit_price numeric(12,4) NOT NULL,
    quantity smallint NOT NULL,
    discount real DEFAULT 0 NOT NULL,
    is_active boolean DEFAULT true,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now(),
    CONSTRAINT order_details_quantity_check CHECK ((quantity > 0))
);


ALTER TABLE public.order_details OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 2442201)
-- Name: orders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.orders (
    order_id integer NOT NULL,
    customer_id character(5),
    employee_id integer,
    order_date timestamp with time zone,
    required_date timestamp with time zone,
    shipped_date timestamp with time zone,
    ship_via integer,
    freight numeric(12,4) DEFAULT 0,
    ship_name character varying(100),
    ship_address character varying(200),
    ship_city character varying(50),
    ship_region character varying(50),
    ship_postal_code character varying(20),
    ship_country character varying(50),
    is_active boolean DEFAULT true,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now(),
    created_by integer,
    updated_by integer
);


ALTER TABLE public.orders OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 2442200)
-- Name: orders_order_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.orders_order_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.orders_order_id_seq OWNER TO postgres;

--
-- TOC entry 3517 (class 0 OID 0)
-- Dependencies: 226
-- Name: orders_order_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.orders_order_id_seq OWNED BY public.orders.order_id;


--
-- TOC entry 225 (class 1259 OID 2442173)
-- Name: products; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.products (
    product_id integer NOT NULL,
    product_name character varying(100) NOT NULL,
    supplier_id integer,
    category_id integer,
    quantity_per_unit character varying(50),
    unit_price numeric(12,4) DEFAULT 0,
    units_in_stock smallint DEFAULT 0,
    units_on_order smallint DEFAULT 0,
    reorder_level smallint DEFAULT 0,
    discontinued boolean DEFAULT false,
    search_vector tsvector GENERATED ALWAYS AS (to_tsvector('spanish'::regconfig, (((product_name)::text || ' '::text) || (COALESCE(quantity_per_unit, ''::character varying))::text))) STORED,
    is_active boolean DEFAULT true,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now(),
    created_by integer,
    updated_by integer
);


ALTER TABLE public.products OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 2442172)
-- Name: products_product_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.products_product_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.products_product_id_seq OWNER TO postgres;

--
-- TOC entry 3518 (class 0 OID 0)
-- Dependencies: 224
-- Name: products_product_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.products_product_id_seq OWNED BY public.products.product_id;


--
-- TOC entry 220 (class 1259 OID 2442136)
-- Name: shippers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.shippers (
    shipper_id integer NOT NULL,
    company_name character varying(100) NOT NULL,
    phone character varying(25),
    is_active boolean DEFAULT true,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now()
);


ALTER TABLE public.shippers OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 2442135)
-- Name: shippers_shipper_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.shippers_shipper_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.shippers_shipper_id_seq OWNER TO postgres;

--
-- TOC entry 3519 (class 0 OID 0)
-- Dependencies: 219
-- Name: shippers_shipper_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.shippers_shipper_id_seq OWNED BY public.shippers.shipper_id;


--
-- TOC entry 218 (class 1259 OID 2442124)
-- Name: suppliers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.suppliers (
    supplier_id integer NOT NULL,
    company_name character varying(100) NOT NULL,
    contact_name character varying(100),
    contact_title character varying(50),
    address character varying(200),
    city character varying(50),
    region character varying(50),
    postal_code character varying(20),
    country character varying(50),
    phone character varying(25),
    fax character varying(25),
    home_page text,
    is_active boolean DEFAULT true,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now(),
    created_by integer,
    updated_by integer
);


ALTER TABLE public.suppliers OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 2442123)
-- Name: suppliers_supplier_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.suppliers_supplier_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.suppliers_supplier_id_seq OWNER TO postgres;

--
-- TOC entry 3520 (class 0 OID 0)
-- Dependencies: 217
-- Name: suppliers_supplier_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.suppliers_supplier_id_seq OWNED BY public.suppliers.supplier_id;


--
-- TOC entry 3256 (class 2604 OID 2442115)
-- Name: categories category_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories ALTER COLUMN category_id SET DEFAULT nextval('public.categories_category_id_seq'::regclass);


--
-- TOC entry 3271 (class 2604 OID 2442159)
-- Name: employees employee_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees ALTER COLUMN employee_id SET DEFAULT nextval('public.employees_employee_id_seq'::regclass);


--
-- TOC entry 3285 (class 2604 OID 2442204)
-- Name: orders order_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders ALTER COLUMN order_id SET DEFAULT nextval('public.orders_order_id_seq'::regclass);


--
-- TOC entry 3275 (class 2604 OID 2442176)
-- Name: products product_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products ALTER COLUMN product_id SET DEFAULT nextval('public.products_product_id_seq'::regclass);


--
-- TOC entry 3264 (class 2604 OID 2442139)
-- Name: shippers shipper_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.shippers ALTER COLUMN shipper_id SET DEFAULT nextval('public.shippers_shipper_id_seq'::regclass);


--
-- TOC entry 3260 (class 2604 OID 2442127)
-- Name: suppliers supplier_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.suppliers ALTER COLUMN supplier_id SET DEFAULT nextval('public.suppliers_supplier_id_seq'::regclass);


--
-- TOC entry 3496 (class 0 OID 2442112)
-- Dependencies: 216
-- Data for Name: categories; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.categories (category_id, category_name, description, picture, is_active, created_at, updated_at, created_by, updated_by) FROM stdin;
1	Motor	Componentes internos y externos del bloque del motor	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
2	Frenos	Discos, pastillas, cilindros y sistemas de frenado	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
3	Suspensión	Amortiguadores, resortes y brazos de control	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
4	Iluminación	Faros principales, stop, neblineros y bombillería	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
5	Transmisión	Cajas de cambio, embragues y diferenciales	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
6	Carrocería	Puertas, capós, parachoques y espejos	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
7	Electrónica	Sensores, ECUs, cables y baterías	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
8	Filtración	Filtros de aceite, aire, combustible y cabina	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
9	Neumáticos	Llantas para todo terreno y ciudad	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
10	Accesorios	Tapetes, fundas y equipamiento interior	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
\.


--
-- TOC entry 3501 (class 0 OID 2442145)
-- Dependencies: 221
-- Data for Name: customers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.customers (customer_id, company_name, contact_name, contact_title, address, city, region, postal_code, country, phone, fax, is_active, created_at, updated_at) FROM stdin;
TALL1	Taller Los Pistones	\N	\N	\N	Madrid	\N	\N	España	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
CONC1	Concesionario Norte	\N	\N	\N	Bogotá	\N	\N	Colombia	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
REPU1	Repuestos El Rayo	\N	\N	\N	Ciudad de México	\N	\N	México	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
MAES1	Maestros del Motor	\N	\N	\N	Buenos Aires	\N	\N	Argentina	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
AUTO1	AutoZone Local	\N	\N	\N	Santiago	\N	\N	Chile	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
SERV1	Servicio Técnico Integral	\N	\N	\N	Lima	\N	\N	Perú	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
PROG1	Pro-Garage Custom	\N	\N	\N	Miami	\N	\N	USA	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
FAST1	Fast Lane Service	\N	\N	\N	Londres	\N	\N	UK	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
EURO1	EuroAuto Parts	\N	\N	\N	Berlín	\N	\N	Alemania	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
JAP01	Japan Tech Experts	\N	\N	\N	Tokio	\N	\N	Japón	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
\.


--
-- TOC entry 3503 (class 0 OID 2442156)
-- Dependencies: 223
-- Data for Name: employees; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.employees (employee_id, last_name, first_name, title, title_of_courtesy, birth_date, hire_date, address, city, region, postal_code, country, home_phone, extension, photo, notes, reports_to, is_active, created_at, updated_at, username, password_hash) FROM stdin;
1	Pérez	Juan	Gerente de Ventas	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-16 05:57:42.253587-05	juan.pérez	$2a$11$3uab86B0K1741tu0mfDeIesWxo9480/a94SUK.igOln8rwg8azDW6
2	García	María	Asesora Técnica	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-16 05:57:42.253587-05	maría.garcía	$2a$11$3uab86B0K1741tu0mfDeIesWxo9480/a94SUK.igOln8rwg8azDW6
3	Rodríguez	Carlos	Encargado de Almacén	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-16 05:57:42.253587-05	carlos.rodríguez	$2a$11$3uab86B0K1741tu0mfDeIesWxo9480/a94SUK.igOln8rwg8azDW6
4	López	Ana	Especialista en Compras	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-16 05:57:42.253587-05	ana.lópez	$2a$11$3uab86B0K1741tu0mfDeIesWxo9480/a94SUK.igOln8rwg8azDW6
5	Martínez	Luis	Soporte Logístico	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-16 05:57:42.253587-05	luis.martínez	$2a$11$3uab86B0K1741tu0mfDeIesWxo9480/a94SUK.igOln8rwg8azDW6
6	Sánchez	Elena	Ventas Internacionales	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-16 05:57:42.253587-05	elena.sánchez	$2a$11$3uab86B0K1741tu0mfDeIesWxo9480/a94SUK.igOln8rwg8azDW6
7	Ramírez	Jorge	Control de Calidad	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-16 05:57:42.253587-05	jorge.ramírez	$2a$11$3uab86B0K1741tu0mfDeIesWxo9480/a94SUK.igOln8rwg8azDW6
8	Torres	Sofía	Atención al Cliente	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-16 05:57:42.253587-05	sofía.torres	$2a$11$3uab86B0K1741tu0mfDeIesWxo9480/a94SUK.igOln8rwg8azDW6
9	Vargas	Andrés	Despachador	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-16 05:57:42.253587-05	andrés.vargas	$2a$11$3uab86B0K1741tu0mfDeIesWxo9480/a94SUK.igOln8rwg8azDW6
10	Castro	Lucía	Analista de Inventario	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-16 05:57:42.253587-05	lucía.castro	$2a$11$3uab86B0K1741tu0mfDeIesWxo9480/a94SUK.igOln8rwg8azDW6
\.


--
-- TOC entry 3508 (class 0 OID 2442226)
-- Dependencies: 228
-- Data for Name: order_details; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.order_details (order_id, product_id, unit_price, quantity, discount, is_active, created_at, updated_at) FROM stdin;
\.


--
-- TOC entry 3507 (class 0 OID 2442201)
-- Dependencies: 227
-- Data for Name: orders; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.orders (order_id, customer_id, employee_id, order_date, required_date, shipped_date, ship_via, freight, ship_name, ship_address, ship_city, ship_region, ship_postal_code, ship_country, is_active, created_at, updated_at, created_by, updated_by) FROM stdin;
1	TALL1	1	2026-04-15 08:28:31.2388-05	\N	\N	1	0.0000	Taller Los Pistones	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
2	CONC1	2	2026-04-15 08:28:31.2388-05	\N	\N	5	0.0000	Concesionario Norte	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
3	REPU1	3	2026-04-15 08:28:31.2388-05	\N	\N	2	0.0000	Repuestos El Rayo	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
4	MAES1	1	2026-04-15 08:28:31.2388-05	\N	\N	3	0.0000	Maestros del Motor	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
5	AUTO1	4	2026-04-15 08:28:31.2388-05	\N	\N	6	0.0000	AutoZone Local	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
6	SERV1	2	2026-04-15 08:28:31.2388-05	\N	\N	4	0.0000	Servicio Técnico Integral	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
7	PROG1	5	2026-04-15 08:28:31.2388-05	\N	\N	1	0.0000	Pro-Garage Custom	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
8	FAST1	1	2026-04-15 08:28:31.2388-05	\N	\N	7	0.0000	Fast Lane Service	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
9	EURO1	6	2026-04-15 08:28:31.2388-05	\N	\N	5	0.0000	EuroAuto Parts	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
10	JAP01	2	2026-04-15 08:28:31.2388-05	\N	\N	8	0.0000	Japan Tech Experts	\N	\N	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
\.


--
-- TOC entry 3505 (class 0 OID 2442173)
-- Dependencies: 225
-- Data for Name: products; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.products (product_id, product_name, supplier_id, category_id, quantity_per_unit, unit_price, units_in_stock, units_on_order, reorder_level, discontinued, is_active, created_at, updated_at, created_by, updated_by) FROM stdin;
1	Pastillas de Freno Cerámicas	2	2	\N	85.5000	150	0	0	f	t	2026-04-15 08:28:31-05	2026-04-15 08:28:31-05	\N	\N
2	Filtro de Aceite Sintético	1	8	\N	12.9900	500	0	0	f	t	2026-04-15 08:28:31-05	2026-04-15 08:28:31-05	\N	\N
3	Batería 12V High Performance	9	7	\N	145.0000	40	0	0	f	t	2026-04-15 08:28:31-05	2026-04-15 08:28:31-05	\N	\N
4	Amortiguador Delantero Gas	5	3	\N	110.0000	80	0	0	f	t	2026-04-15 08:28:31-05	2026-04-15 08:28:31-05	\N	\N
5	Bujía de Iridio (Set x4)	4	1	\N	45.0000	200	0	0	f	t	2026-04-15 08:28:31-05	2026-04-15 08:28:31-05	\N	\N
6	Kit de Embrague Reforzado	7	5	\N	320.0000	25	0	0	f	t	2026-04-15 08:28:31-05	2026-04-15 08:28:31-05	\N	\N
7	Faro LED Principal Izquierdo	8	4	\N	210.0000	15	0	0	f	t	2026-04-15 08:28:31-05	2026-04-15 08:28:31-05	\N	\N
8	Neumático All-Season 205/55R16	3	9	\N	95.0000	100	0	0	f	t	2026-04-15 08:28:31-05	2026-04-15 08:28:31-05	\N	\N
9	Correa de Tiempo Dentada	10	1	\N	65.0000	60	0	0	f	t	2026-04-15 08:28:31-05	2026-04-15 08:28:31-05	\N	\N
10	Bomba de Agua Eléctrica	6	1	\N	180.0000	30	0	0	f	t	2026-04-15 08:28:31-05	2026-04-15 08:28:31-05	\N	\N
\.


--
-- TOC entry 3500 (class 0 OID 2442136)
-- Dependencies: 220
-- Data for Name: shippers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.shippers (shipper_id, company_name, phone, is_active, created_at, updated_at) FROM stdin;
1	AutoEnvío Express	555-0101	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
2	Logística Motores S.A.	555-0202	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
3	Repuestos al Día	555-0303	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
4	Global Cargo Auto	555-0404	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
5	DHL Automotive	555-0505	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
6	FedEx Parts	555-0606	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
7	Rápido y Furioso Logística	555-0707	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
8	Turbo Shipping	555-0808	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
9	Piston Delivery	555-0909	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
10	Vía Terrestre Carga	555-1010	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05
\.


--
-- TOC entry 3498 (class 0 OID 2442124)
-- Dependencies: 218
-- Data for Name: suppliers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.suppliers (supplier_id, company_name, contact_name, contact_title, address, city, region, postal_code, country, phone, fax, home_page, is_active, created_at, updated_at, created_by, updated_by) FROM stdin;
1	Bosch Automotive	Karl Benz	\N	\N	Stuttgart	\N	\N	Alemania	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
2	Brembo S.p.A	Alberto Bombassei	\N	\N	Curno	\N	\N	Italia	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
3	Michelin Group	Florent Menegaux	\N	\N	Clermont-Ferrand	\N	\N	Francia	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
4	Denso Corp	Koji Arima	\N	\N	Kariya	\N	\N	Japón	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
5	ZF Friedrichshafen	Wolf-Henning Scheider	\N	\N	Friedrichshafen	\N	\N	Alemania	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
6	Magna International	Swamy Kotagiri	\N	\N	Aurora	\N	\N	Canadá	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
7	Aisin Seiki	Kiyotaka Ise	\N	\N	Kariya	\N	\N	Japón	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
8	Valeo SA	Christophe Périllat	\N	\N	Paris	\N	\N	Francia	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
9	Continental AG	Nikolai Setzer	\N	\N	Hanover	\N	\N	Alemania	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
10	Gates Corporation	Ivo Jurek	\N	\N	Denver	\N	\N	USA	\N	\N	\N	t	2026-04-15 08:28:31.2388-05	2026-04-15 08:28:31.2388-05	\N	\N
\.


--
-- TOC entry 3521 (class 0 OID 0)
-- Dependencies: 215
-- Name: categories_category_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.categories_category_id_seq', 10, true);


--
-- TOC entry 3522 (class 0 OID 0)
-- Dependencies: 222
-- Name: employees_employee_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.employees_employee_id_seq', 10, true);


--
-- TOC entry 3523 (class 0 OID 0)
-- Dependencies: 226
-- Name: orders_order_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.orders_order_id_seq', 10, true);


--
-- TOC entry 3524 (class 0 OID 0)
-- Dependencies: 224
-- Name: products_product_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.products_product_id_seq', 10, true);


--
-- TOC entry 3525 (class 0 OID 0)
-- Dependencies: 219
-- Name: shippers_shipper_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.shippers_shipper_id_seq', 10, true);


--
-- TOC entry 3526 (class 0 OID 0)
-- Dependencies: 217
-- Name: suppliers_supplier_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.suppliers_supplier_id_seq', 10, true);


--
-- TOC entry 3296 (class 2606 OID 2442122)
-- Name: categories categories_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_pkey PRIMARY KEY (category_id);


--
-- TOC entry 3303 (class 2606 OID 2442154)
-- Name: customers customers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customers
    ADD CONSTRAINT customers_pkey PRIMARY KEY (customer_id);


--
-- TOC entry 3305 (class 2606 OID 2442166)
-- Name: employees employees_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_pkey PRIMARY KEY (employee_id);


--
-- TOC entry 3307 (class 2606 OID 2442264)
-- Name: employees employees_username_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_username_key UNIQUE (username);


--
-- TOC entry 3325 (class 2606 OID 2442235)
-- Name: order_details order_details_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_details
    ADD CONSTRAINT order_details_pkey PRIMARY KEY (order_id, product_id);


--
-- TOC entry 3322 (class 2606 OID 2442210)
-- Name: orders orders_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT orders_pkey PRIMARY KEY (order_id);


--
-- TOC entry 3315 (class 2606 OID 2442189)
-- Name: products products_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_pkey PRIMARY KEY (product_id);


--
-- TOC entry 3301 (class 2606 OID 2442144)
-- Name: shippers shippers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.shippers
    ADD CONSTRAINT shippers_pkey PRIMARY KEY (shipper_id);


--
-- TOC entry 3299 (class 2606 OID 2442134)
-- Name: suppliers suppliers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.suppliers
    ADD CONSTRAINT suppliers_pkey PRIMARY KEY (supplier_id);


--
-- TOC entry 3297 (class 1259 OID 2442327)
-- Name: idx_categories_created_by; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_categories_created_by ON public.categories USING btree (created_by);


--
-- TOC entry 3308 (class 1259 OID 2442276)
-- Name: idx_employees_login; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_employees_login ON public.employees USING btree (username, is_active);


--
-- TOC entry 3309 (class 1259 OID 2442265)
-- Name: idx_employees_username; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_employees_username ON public.employees USING btree (username);


--
-- TOC entry 3323 (class 1259 OID 2442251)
-- Name: idx_order_details_product; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_order_details_product ON public.order_details USING btree (product_id);


--
-- TOC entry 3316 (class 1259 OID 2442294)
-- Name: idx_orders_audit_created; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_orders_audit_created ON public.orders USING btree (created_by);


--
-- TOC entry 3317 (class 1259 OID 2442328)
-- Name: idx_orders_created_by; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_orders_created_by ON public.orders USING btree (created_by);


--
-- TOC entry 3318 (class 1259 OID 2442249)
-- Name: idx_orders_customer_date; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_orders_customer_date ON public.orders USING btree (customer_id, order_date DESC);


--
-- TOC entry 3319 (class 1259 OID 2442250)
-- Name: idx_orders_date_brin; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_orders_date_brin ON public.orders USING brin (order_date);


--
-- TOC entry 3320 (class 1259 OID 2442295)
-- Name: idx_orders_employee_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_orders_employee_id ON public.orders USING btree (employee_id);


--
-- TOC entry 3310 (class 1259 OID 2442247)
-- Name: idx_products_category_price; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_products_category_price ON public.products USING btree (category_id, unit_price) WHERE (is_active = true);


--
-- TOC entry 3311 (class 1259 OID 2442326)
-- Name: idx_products_created_by; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_products_created_by ON public.products USING btree (created_by);


--
-- TOC entry 3312 (class 1259 OID 2442248)
-- Name: idx_products_name_trgm; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_products_name_trgm ON public.products USING gin (product_name public.gin_trgm_ops);


--
-- TOC entry 3313 (class 1259 OID 2442246)
-- Name: idx_products_search_gin; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_products_search_gin ON public.products USING gin (search_vector);


--
-- TOC entry 3345 (class 2620 OID 2442255)
-- Name: categories tr_set_timestamp_categories; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_set_timestamp_categories BEFORE UPDATE ON public.categories FOR EACH ROW EXECUTE FUNCTION public.trigger_set_timestamp();


--
-- TOC entry 3348 (class 2620 OID 2442256)
-- Name: customers tr_set_timestamp_customers; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_set_timestamp_customers BEFORE UPDATE ON public.customers FOR EACH ROW EXECUTE FUNCTION public.trigger_set_timestamp();


--
-- TOC entry 3349 (class 2620 OID 2442252)
-- Name: employees tr_set_timestamp_employees; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_set_timestamp_employees BEFORE UPDATE ON public.employees FOR EACH ROW EXECUTE FUNCTION public.trigger_set_timestamp();


--
-- TOC entry 3352 (class 2620 OID 2442259)
-- Name: order_details tr_set_timestamp_order_details; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_set_timestamp_order_details BEFORE UPDATE ON public.order_details FOR EACH ROW EXECUTE FUNCTION public.trigger_set_timestamp();


--
-- TOC entry 3351 (class 2620 OID 2442257)
-- Name: orders tr_set_timestamp_orders; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_set_timestamp_orders BEFORE UPDATE ON public.orders FOR EACH ROW EXECUTE FUNCTION public.trigger_set_timestamp();


--
-- TOC entry 3350 (class 2620 OID 2442254)
-- Name: products tr_set_timestamp_products; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_set_timestamp_products BEFORE UPDATE ON public.products FOR EACH ROW EXECUTE FUNCTION public.trigger_set_timestamp();


--
-- TOC entry 3347 (class 2620 OID 2442258)
-- Name: shippers tr_set_timestamp_shippers; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_set_timestamp_shippers BEFORE UPDATE ON public.shippers FOR EACH ROW EXECUTE FUNCTION public.trigger_set_timestamp();


--
-- TOC entry 3346 (class 2620 OID 2442253)
-- Name: suppliers tr_set_timestamp_suppliers; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_set_timestamp_suppliers BEFORE UPDATE ON public.suppliers FOR EACH ROW EXECUTE FUNCTION public.trigger_set_timestamp();


--
-- TOC entry 3329 (class 2606 OID 2442167)
-- Name: employees employees_reports_to_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_reports_to_fkey FOREIGN KEY (reports_to) REFERENCES public.employees(employee_id);


--
-- TOC entry 3326 (class 2606 OID 2442271)
-- Name: categories fk_cat_emp_created; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT fk_cat_emp_created FOREIGN KEY (created_by) REFERENCES public.employees(employee_id);


--
-- TOC entry 3327 (class 2606 OID 2442306)
-- Name: categories fk_categories_employee_created; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT fk_categories_employee_created FOREIGN KEY (created_by) REFERENCES public.employees(employee_id);


--
-- TOC entry 3328 (class 2606 OID 2442311)
-- Name: categories fk_categories_employee_updated; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT fk_categories_employee_updated FOREIGN KEY (updated_by) REFERENCES public.employees(employee_id);


--
-- TOC entry 3335 (class 2606 OID 2442284)
-- Name: orders fk_orders_created_by; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_created_by FOREIGN KEY (created_by) REFERENCES public.employees(employee_id);


--
-- TOC entry 3336 (class 2606 OID 2442279)
-- Name: orders fk_orders_employee; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_employee FOREIGN KEY (employee_id) REFERENCES public.employees(employee_id);


--
-- TOC entry 3337 (class 2606 OID 2442316)
-- Name: orders fk_orders_employee_created; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_employee_created FOREIGN KEY (created_by) REFERENCES public.employees(employee_id);


--
-- TOC entry 3338 (class 2606 OID 2442321)
-- Name: orders fk_orders_employee_updated; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_employee_updated FOREIGN KEY (updated_by) REFERENCES public.employees(employee_id);


--
-- TOC entry 3339 (class 2606 OID 2442289)
-- Name: orders fk_orders_updated_by; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_updated_by FOREIGN KEY (updated_by) REFERENCES public.employees(employee_id);


--
-- TOC entry 3330 (class 2606 OID 2442266)
-- Name: products fk_prod_emp_created; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT fk_prod_emp_created FOREIGN KEY (created_by) REFERENCES public.employees(employee_id);


--
-- TOC entry 3331 (class 2606 OID 2442296)
-- Name: products fk_products_employee_created; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT fk_products_employee_created FOREIGN KEY (created_by) REFERENCES public.employees(employee_id);


--
-- TOC entry 3332 (class 2606 OID 2442301)
-- Name: products fk_products_employee_updated; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT fk_products_employee_updated FOREIGN KEY (updated_by) REFERENCES public.employees(employee_id);


--
-- TOC entry 3343 (class 2606 OID 2442236)
-- Name: order_details order_details_order_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_details
    ADD CONSTRAINT order_details_order_id_fkey FOREIGN KEY (order_id) REFERENCES public.orders(order_id) ON DELETE CASCADE;


--
-- TOC entry 3344 (class 2606 OID 2442241)
-- Name: order_details order_details_product_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_details
    ADD CONSTRAINT order_details_product_id_fkey FOREIGN KEY (product_id) REFERENCES public.products(product_id) ON DELETE CASCADE;


--
-- TOC entry 3340 (class 2606 OID 2442211)
-- Name: orders orders_customer_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT orders_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES public.customers(customer_id) ON DELETE CASCADE;


--
-- TOC entry 3341 (class 2606 OID 2442216)
-- Name: orders orders_employee_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT orders_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES public.employees(employee_id);


--
-- TOC entry 3342 (class 2606 OID 2442221)
-- Name: orders orders_ship_via_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT orders_ship_via_fkey FOREIGN KEY (ship_via) REFERENCES public.shippers(shipper_id);


--
-- TOC entry 3333 (class 2606 OID 2442195)
-- Name: products products_category_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_category_id_fkey FOREIGN KEY (category_id) REFERENCES public.categories(category_id) ON DELETE CASCADE;


--
-- TOC entry 3334 (class 2606 OID 2442190)
-- Name: products products_supplier_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_supplier_id_fkey FOREIGN KEY (supplier_id) REFERENCES public.suppliers(supplier_id) ON DELETE SET NULL;


-- Completed on 2026-04-16 09:08:44

--
-- PostgreSQL database dump complete
--

