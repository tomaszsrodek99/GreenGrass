function showLoadingIcon() {
    $('#blurOverlay').show();
    $('body').addClass('blur-overlay-visible');
    $('<i class="fas fa-spinner fa-spin loading-spinner"></i>').css('display', 'block').appendTo('body');
}
function hideLoadingIcon() {
    $('.loading-spinner').remove();
    $('#blurOverlay').hide();
    $('body').removeClass('blur-overlay-visible');
}

let shouldShowLoading = false; 

function loading() {
    if (shouldShowLoading) {
        showLoadingIcon();
        setTimeout(async function () {
            hideLoadingIcon();
        }, 9999);

        const allSpans = document.getElementsByTagName("span");
        for (const span of allSpans) {
            if (span.classList.contains("field-validation-error")) {
                hideLoadingIcon();
                break;
            }
        }
    }
}

window.addEventListener('beforeunload', () => {
    shouldShowLoading = true;   
    loading();
});

function goBack() {
    history.back();
}

function hideModal() {
    var notificationContainer = document.querySelector('.notification-overlay');
    notificationContainer.style.display = 'none';
}

function searchByName() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("searchInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("searchTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}


