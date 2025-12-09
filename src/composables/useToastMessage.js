import { useToast } from "vue-toastification";

export function useToastMessage() {
   const toast = useToast();

   function showToastSuccess(message) {
      toast.success(message, {
         position: "top-center",
         timeout: 3000,
      });
   }

   function showToastError(message) {
      toast.error(message, {
         position: "top-center",
         timeout: 3000,
      });
   }

   function showToastInfo(message) {
      toast.error(message, {
         position: "top-center",
         timeout: 3000,
      });
   }

   return {
      showToastSuccess,
      showToastError,
      showToastInfo
   };
}
