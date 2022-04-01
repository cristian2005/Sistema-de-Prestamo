
    $(document).ready(function () {
        $("#enviar").on("click", function (e) {
            if (validarCuota(["Monto", "NoCuotas", "FechaInicio", "FormaPago"])) {
                let formCuotas = $('#form-cuotas').serializeArray();

                formCuotas = formatFormData(formCuotas, ["noCuota[]", "fecha_pago[]", "interes[]", "capital[]", "restante[]"]);
                let cuotas = [];
                formCuotas.capital.forEach(function (t, index) {
                    cuotas.push({
                        Id: 0, fecha_pago: formCuotas.fecha_pago[index], noCuota: formCuotas.noCuota[index], interes: formCuotas.interes[index], capital: t,
                        restante: formCuotas.restante[index], Prestamod_Id: 0
                    });
                });
                let prestamos = formatFormData($("#form-prestamos").serializeArray(), ["Monto", "NoCuotas", "FechaInicio", "FormaPago", "MontoCuota", "TotalIntereses", "MontoPagar", "Cliente_Id", "Prestadore_Id", "Interes"]);
                prestamos.Cuotas = cuotas;

                $.ajax({
                    type: "POST",
                    url: "/Prestamo/Create",
                    data: JSON.stringify(prestamos),
                    headers: { "Content-Type": "application/json" },
                    dataType: "json",
                    success: function (resp) {
                        console.log(resp);
                        if (resp.Code == -2) {
                            alert("No puedes adquirir este prestamo porque no cuenta con los suficientes fondo para pagar la cuota");
                        }else
                        window.location = "Index";
                    },
                });
            }
            });
    });
function GenerarCuota() {

    if (validarCuota(["Monto", "NoCuotas", "FechaInicio", "FormaPago"])) {

        let monto = parseFloat($('#Monto').val());
        let numCuota = parseInt($('#NoCuotas').val());
        let fecha = $('#FechaInicio').val();
        let formaPago = $('#FormaPago').val();
        let html = "";
        let total_interes = 0.00;
		var interestRate =
			parseFloat(document.getElementById("Interes").value / 100.0);

		var monthlyRate = interestRate / 12;

		//Calculate the payment
		var payment = monto * (monthlyRate / (1 - Math.pow(
			1 + monthlyRate, -numCuota)));

		$('#MontoCuota').val(payment.toFixed(2));
		$('#MontoPagar').val((payment * numCuota).toFixed(2));

        for (var i = 0; i <= numCuota; i++) {

			let fecha_pago = "";
			//in-loop interest amount holder
			var interest = 0;

			//in-loop monthly principal amount holder
			var monthlyPrincipal = 0;

			//calc the in-loop interest amount and display
			interest = monto * monthlyRate;
            total_interes += interest;
			//calc the in-loop monthly principal and display
			monthlyPrincipal = payment - interest;
			//result += "<td> $" + monthlyPrincipal.toFixed(2) + "</td>";

            switch (formaPago) {
                case 'Diario':
                    fecha_pago = moment(fecha).add("day", i).format("DD/MM/YYYY");
                    break;
                case 'Semanal':
                    fecha_pago = moment(fecha).add("day", (i*7)).format("DD/MM/YYYY");
                    break;
                case 'Quincenal':
                    fecha_pago = moment(fecha).add("day", i*15).format("DD/MM/YYYY");
                    break;
                case 'Mensual':
                    fecha_pago = moment(fecha).add("month", i).format("DD/MM/YYYY");
                    break;
                case 'Anual':
                    fecha_pago = moment(fecha).add("year", i).format("DD/MM/YYYY");
                    break;
                default:
            }
            
            html += `
               <tr>
                <th>${i}<input type="hidden" value="${i}" name="noCuota[]"></th>
                <th>${fecha_pago}<input type="hidden" value="${fecha_pago}" name="fecha_pago[]"></th>
                <th>${interest.toFixed(2)}<input type="hidden" value="${interest.toFixed(2)}" name="interes[]"></th>
                <th>${monthlyPrincipal.toFixed(2)}<input type="hidden" value="${monthlyPrincipal.toFixed(2)}" name="capital[]"></th>
                <th>${monto.toFixed(2)}<input type="hidden" value="${monto.toFixed(2)}" name="restante[]"></th>
               </tr>
            `;
            //update the balance for each loop iteration
			monto = monto - monthlyPrincipal;
        }
        $('#TotalIntereses').val(total_interes.toFixed(2));
        $('#tablaCuotas tbody').html(html);
        $('#modalCuotas').modal("show");
    }
}
function validarCuota(arrayId) {
    for (var i = 0 ; i<=arrayId.length; i++) {
        if ($("#"+arrayId[i]).val()=="") {
            alert("El campo "+arrayId[i] + " es obligatorio para generar la cuota");
            return false;
            break;
        }
    }
    return true;
}
/**
    * Da formato a un formulario que este serializeArray eje: data_form={name:nombre,value:Cristian}, columnSearch=['nombre'], return {nombre:Cristian}
    * @param {Array} data_form La data del formulario
    * @param {string} columnSearch Espeficicar en un array las columnas a formatear
    * @param {boolean} add_object true si quieres agregar la data devuelta al objeto instanciado.
    * @returns {object} Devuelve el objeto el ejemplo anterior {nombre:Cristian}.
    */
formatFormData = (data_form, columnSearch, add_object = false) => {
    let array_object = [];

    let result = data_form
        .filter((item) => columnSearch.includes(item.name))
        .map(function (x) {
            if (x.name.includes("[]")) {
                let name_object = x.name.substring(0, x.name.indexOf("[]"));
                array_object.push({ [name_object]: x.value });
                let data_object = array_object
                    .map((x) => x[name_object])
                    .filter((x) => x !== undefined);
                return { [name_object]: data_object };
            }
            return { [x.name]: x.value };
        });
    result = Object.assign({}, ...result);
    return add_object ? this.insert(result) : result;
};