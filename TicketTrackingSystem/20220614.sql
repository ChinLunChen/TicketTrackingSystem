PGDMP                         z            postgres    13.1    13.1     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    13442    postgres    DATABASE     r   CREATE DATABASE postgres WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Chinese (Traditional)_Taiwan.950';
    DROP DATABASE postgres;
                postgres    false            �           0    0    DATABASE postgres    COMMENT     N   COMMENT ON DATABASE postgres IS 'default administrative connection database';
                   postgres    false    2994                        3079    16384 	   adminpack 	   EXTENSION     A   CREATE EXTENSION IF NOT EXISTS adminpack WITH SCHEMA pg_catalog;
    DROP EXTENSION adminpack;
                   false            �           0    0    EXTENSION adminpack    COMMENT     M   COMMENT ON EXTENSION adminpack IS 'administrative functions for PostgreSQL';
                        false    2            �            1259    24593    buglist    TABLE     �  CREATE TABLE public.buglist (
    id character varying(20) NOT NULL,
    summary character varying(50) NOT NULL,
    description character varying(150) NOT NULL,
    type character varying(20) NOT NULL,
    priority character varying(10) NOT NULL,
    status character varying(10) NOT NULL,
    create_user character varying(10) NOT NULL,
    create_date date NOT NULL,
    end_user character varying(10),
    end_date date
);
    DROP TABLE public.buglist;
       public         heap    postgres    false            �            1259    24576 	   user_info    TABLE     �   CREATE TABLE public.user_info (
    "UserID" character varying(50) NOT NULL,
    "UserName" character varying(50),
    "UserType" character varying(50)
);
    DROP TABLE public.user_info;
       public         heap    postgres    false            �          0    24593    buglist 
   TABLE DATA           �   COPY public.buglist (id, summary, description, type, priority, status, create_user, create_date, end_user, end_date) FROM stdin;
    public          postgres    false    202   s       �          0    24576 	   user_info 
   TABLE DATA           E   COPY public.user_info ("UserID", "UserName", "UserType") FROM stdin;
    public          postgres    false    201   ;       &           2606    24580    user_info User_pkey 
   CONSTRAINT     Y   ALTER TABLE ONLY public.user_info
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("UserID");
 ?   ALTER TABLE ONLY public.user_info DROP CONSTRAINT "User_pkey";
       public            postgres    false    201            (           2606    24601    buglist buglist_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.buglist
    ADD CONSTRAINT buglist_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.buglist DROP CONSTRAINT buglist_pkey;
       public            postgres    false    202            �   �   x�����0���S��u�H�M��ˢ��(D����Иx������߿\�Q�RK���n�\���]%����3�3�=�:F��&�|�R 
	��D��D�5��^�0�g�5l������#;x���Nd��m���5Jq�<�>��(7�~=&��(�}R8]�3f��J��C�ɉ�Ղ1�u�n�      �   '   x�r�!� _N0�
t�#.ǔ��<N$�+F��� ,�#     