CREATE OR REPLACE FUNCTION actualizar_articulos_almacen(
														opcion SMALLINT, 
														nCategoria INTEGER, 
														nFolio INTEGER,
														nCantidad INTEGER,
														sRequisicion VARCHAR(20), 
														sObservacion VARCHAR(100))
RETURNS INTEGER AS
$$

	DECLARE
		 nId INTEGER;
		 nFolioMovto INTEGER;

	BEGIN
	
		SELECT Id
		INTO nId
		FROM tb_categorias
		WHERE numero_categoria = nCategoria;
		
		nFolioMovto := (SELECT generar_folio_movimiento(opcion));
	
		IF opcion = 0 THEN
		
			UPDATE tb_articulosalmacen
			SET cantidad = cantidad + nCantidad, folio_movimiento = nFolioMovto
			WHERE folio_articulo = nFolio AND numero_categoria_id = nId;
			
			UPDATE tb_foliador
			SET folio = nFolioMovto
			WHERE id_identificador = opcion;
		
		ELSIF opcion = 1 THEN
		
			UPDATE tb_articulosalmacen
			SET cantidad = cantidad - nCantidad, folio_movimiento = nFolioMovto
			WHERE folio_articulo = nFolio AND numero_categoria_id = nId;
			
			UPDATE tb_foliador
			SET folio = nFolioMovto
			WHERE id_identificador = opcion;
		
		END IF;
	
		RETURN nFolioMovto;
	END

$$
LANGUAGE 'plpgsql';