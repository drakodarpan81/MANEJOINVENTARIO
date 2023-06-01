-- DROP FUNCTION cmb_mostrar_informacion_articulos();
-- DROP TYPE type_informacion_articulos_almacen;

CREATE TYPE type_informacion_articulos_almacen AS 
(
	cantidad INTEGER,
	stock INTEGER
);

CREATE OR REPLACE FUNCTION cmb_mostrar_informacion_articulos(nCategoria INTEGER, nFolio INTEGER)
RETURNS SETOF type_informacion_articulos_almacen AS
$$

	DECLARE
		reg RECORD;
		nId INTEGER;

	BEGIN
		
		SELECT Id
		INTO nId
		FROM tb_categorias
		WHERE numero_categoria = nCategoria;
	
		FOR reg IN  SELECT cantidad, stock
					FROM tb_articulosalmacen 
					WHERE folio_articulo = nFolio AND numero_categoria_id = nId
		LOOP
			RETURN NEXT reg;
		END LOOP;
		RETURN;
	
	END

$$
LANGUAGE 'plpgsql';
