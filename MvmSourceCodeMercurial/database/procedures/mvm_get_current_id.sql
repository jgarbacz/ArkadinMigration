CREATE OR REPLACE PROCEDURE mvm_get_current_id (v_nm_current varchar2, v_quantity integer, v_current_id OUT integer)
AS
PRAGMA AUTONOMOUS_TRANSACTION;
BEGIN

	select id_current into v_current_id from t_current_id where nm_current = v_nm_current for update;
	update t_current_id set id_current = id_current + v_quantity where nm_current = v_nm_current;
	commit;
END;
/

